const http = require('http');
const fs = require('fs');
const path = require('path');
const crypto = require('crypto');

const PORT = 5173;
const FILE_PATH = path.join(__dirname, 'index.html');
const SAVES_DIR = path.join(__dirname, 'saves');
const SAVE_FILE = path.join(SAVES_DIR, 'savegame.json');

// Ensure saves directory exists
if (!fs.existsSync(SAVES_DIR)) {
    fs.mkdirSync(SAVES_DIR, { recursive: true });
}

// In-Memory Multiplayer Room & Player State
const rooms = new Map(); // roomCode -> { code, hostId, createdAt, players: Map(playerId -> state) }
const connectedClients = new Map(); // socket -> { id, roomCode, name }

// Default Global Room
rooms.set('ECL-709', {
    code: 'ECL-709',
    name: 'Greyfall City Central Sector',
    createdAt: Date.now(),
    players: new Map()
});

// Helper: Broadcast WebSocket message to a room
function broadcastToRoom(roomCode, messageObj, excludeSocket = null) {
    const rawData = JSON.stringify(messageObj);
    const frame = encodeWebSocketFrame(rawData);

    for (const [socket, client] of connectedClients.entries()) {
        if (client.roomCode === roomCode && socket !== excludeSocket) {
            try {
                socket.write(frame);
            } catch (err) {
                // Client socket closed
            }
        }
    }
}

// WebSocket Frame Encoder (RFC 6455)
function encodeWebSocketFrame(data) {
    const payload = Buffer.from(data, 'utf8');
    const length = payload.length;

    let header;
    if (length < 126) {
        header = Buffer.alloc(2);
        header[0] = 0x81; // FIN + text frame
        header[1] = length;
    } else if (length <= 0xFFFF) {
        header = Buffer.alloc(4);
        header[0] = 0x81;
        header[1] = 126;
        header.writeUInt16BE(length, 2);
    } else {
        header = Buffer.alloc(10);
        header[0] = 0x81;
        header[1] = 127;
        header.writeBigUInt64BE(BigInt(length), 2);
    }

    return Buffer.concat([header, payload]);
}

// WebSocket Frame Decoder
function decodeWebSocketFrame(buffer) {
    if (buffer.length < 2) return null;

    const firstByte = buffer[0];
    const secondByte = buffer[1];
    const opcode = firstByte & 0x0F;
    const isMasked = (secondByte & 0x80) !== 0;

    if (opcode === 0x08) {
        return { type: 'close' };
    }
    if (opcode === 0x09) {
        return { type: 'ping' };
    }

    let payloadLen = secondByte & 0x7F;
    let offset = 2;

    if (payloadLen === 126) {
        if (buffer.length < 4) return null;
        payloadLen = buffer.readUInt16BE(2);
        offset = 4;
    } else if (payloadLen === 127) {
        if (buffer.length < 10) return null;
        payloadLen = Number(buffer.readBigUInt64BE(2));
        offset = 10;
    }

    if (isMasked) {
        if (buffer.length < offset + 4 + payloadLen) return null;
        const mask = buffer.slice(offset, offset + 4);
        offset += 4;
        const payload = buffer.slice(offset, offset + payloadLen);
        const unmasked = Buffer.alloc(payloadLen);
        for (let i = 0; i < payloadLen; i++) {
            unmasked[i] = payload[i] ^ mask[i % 4];
        }
        return { type: 'text', data: unmasked.toString('utf8'), bytesConsumed: offset + payloadLen };
    } else {
        if (buffer.length < offset + payloadLen) return null;
        const payload = buffer.slice(offset, offset + payloadLen);
        return { type: 'text', data: payload.toString('utf8'), bytesConsumed: offset + payloadLen };
    }
}

// Main HTTP Server
const server = http.createServer((req, res) => {
    // Standard CORS Headers for local tools & mobile connectivity
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type, Authorization');

    if (req.method === 'OPTIONS') {
        res.writeHead(204);
        res.end();
        return;
    }

    const url = new URL(req.url, `http://${req.headers.host || 'localhost'}`);
    const pathname = url.pathname;

    // --- REST API ROUTES ---
    if (pathname === '/api/status') {
        const totalPlayers = connectedClients.size;
        const activeRooms = rooms.size;
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({
            status: 'online',
            service: 'ECLIPSE Master Game Server',
            version: '1.0.0-batchB',
            engine: 'Three.js / WebGL + Unity 6 Bridge',
            port: PORT,
            connectedClients: totalPlayers,
            activeRooms: activeRooms,
            uptimeSeconds: Math.floor(process.uptime()),
            timestamp: Date.now()
        }, null, 2));
        return;
    }

    if (pathname === '/api/rooms') {
        const roomList = [];
        for (const [code, r] of rooms.entries()) {
            roomList.push({
                code: r.code,
                name: r.name,
                playerCount: r.players.size,
                maxPlayers: 4,
                createdAt: r.createdAt
            });
        }
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ rooms: roomList }, null, 2));
        return;
    }

    if (pathname === '/api/rooms/create' && req.method === 'POST') {
        let body = '';
        req.on('data', chunk => { body += chunk; });
        req.on('end', () => {
            let data = {};
            try { data = JSON.parse(body); } catch (e) {}

            const code = 'ECL-' + Math.floor(100 + Math.random() * 900);
            const roomName = data.name || `Greyfall Sector ${code}`;

            rooms.set(code, {
                code: code,
                name: roomName,
                createdAt: Date.now(),
                players: new Map()
            });

            res.writeHead(201, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify({ success: true, roomCode: code, name: roomName }));
        });
        return;
    }

    if (pathname === '/api/save' && req.method === 'POST') {
        let body = '';
        req.on('data', chunk => { body += chunk; });
        req.on('end', () => {
            try {
                const saveData = JSON.parse(body);
                saveData.savedAt = new Date().toISOString();
                fs.writeFileSync(SAVE_FILE, JSON.stringify(saveData, null, 2), 'utf8');
                res.writeHead(200, { 'Content-Type': 'application/json' });
                res.end(JSON.stringify({ success: true, message: 'Game progress successfully saved to backend.' }));
            } catch (err) {
                res.writeHead(400, { 'Content-Type': 'application/json' });
                res.end(JSON.stringify({ success: false, error: err.message }));
            }
        });
        return;
    }

    if (pathname === '/api/load') {
        if (fs.existsSync(SAVE_FILE)) {
            try {
                const data = fs.readFileSync(SAVE_FILE, 'utf8');
                res.writeHead(200, { 'Content-Type': 'application/json' });
                res.end(data);
                return;
            } catch (err) {
                res.writeHead(500, { 'Content-Type': 'application/json' });
                res.end(JSON.stringify({ error: 'Failed to read save file.' }));
                return;
            }
        } else {
            res.writeHead(404, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify({ status: 'no_save_found', message: 'No save file exists yet.' }));
            return;
        }
    }

    if (pathname === '/api/mcp/status') {
        // Query Unity MCP Bridge on port 8080
        http.get('http://127.0.0.1:8080/mcp/?action=ping', (mcpRes) => {
            let mcpData = '';
            mcpRes.on('data', chunk => { mcpData += chunk; });
            mcpRes.on('end', () => {
                res.writeHead(200, { 'Content-Type': 'application/json' });
                res.end(mcpData);
            });
        }).on('error', (err) => {
            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify({ status: 'offline', message: 'Unity MCP Bridge not running on port 8080.', error: err.message }));
        });
        return;
    }

    // --- STATIC ASSET SERVING ---
    fs.readFile(FILE_PATH, (err, data) => {
        if (err) {
            res.writeHead(500, { 'Content-Type': 'text/plain' });
            res.end('Error loading ECLIPSE game client.');
            return;
        }
        res.writeHead(200, { 'Content-Type': 'text/html' });
        res.end(data);
    });
});

// WebSocket Protocol Upgrade Handler (RFC 6455)
server.on('upgrade', (req, socket, head) => {
    const key = req.headers['sec-websocket-key'];
    if (!key) {
        socket.destroy();
        return;
    }

    const sha1 = crypto.createHash('sha1');
    sha1.update(key + '258EAFA5-E914-47DA-95CA-C5AB0DC85B11');
    const acceptKey = sha1.digest('base64');

    const responseHeaders = [
        'HTTP/1.1 101 Switching Protocols',
        'Upgrade: websocket',
        'Connection: Upgrade',
        `Sec-WebSocket-Accept: ${acceptKey}`
    ];

    socket.write(responseHeaders.join('\r\n') + '\r\n\r\n');

    const clientId = 'player_' + Math.floor(1000 + Math.random() * 9000);
    const clientRoom = 'ECL-709';

    connectedClients.set(socket, {
        id: clientId,
        roomCode: clientRoom,
        name: `Ethan Cross (${clientId.slice(-4)})`
    });

    console.log(`[WebSocket] Client connected: ${clientId} joined room ${clientRoom} (Total: ${connectedClients.size})`);

    // Acknowledge connection to client
    const welcomeMsg = encodeWebSocketFrame(JSON.stringify({
        type: 'welcome',
        clientId: clientId,
        roomCode: clientRoom,
        activePlayers: Array.from(rooms.get(clientRoom)?.players.values() || [])
    }));
    socket.write(welcomeMsg);

    // Buffer for chunked WebSocket frames
    let buffer = Buffer.alloc(0);

    socket.on('data', (chunk) => {
        buffer = Buffer.concat([buffer, chunk]);

        while (buffer.length > 0) {
            const frame = decodeWebSocketFrame(buffer);
            if (!frame) break;

            if (frame.type === 'close') {
                handleClientDisconnect(socket);
                socket.end();
                return;
            }

            if (frame.type === 'ping') {
                const pongFrame = Buffer.from([0x8A, 0x00]);
                socket.write(pongFrame);
            }

            if (frame.type === 'text') {
                try {
                    const msg = JSON.parse(frame.data);
                    handleClientMessage(socket, msg);
                } catch (e) {
                    // Invalid JSON
                }
            }

            buffer = buffer.slice(frame.bytesConsumed || buffer.length);
        }
    });

    socket.on('close', () => {
        handleClientDisconnect(socket);
    });

    socket.on('error', () => {
        handleClientDisconnect(socket);
    });
});

function handleClientMessage(socket, msg) {
    const client = connectedClients.get(socket);
    if (!client) return;

    switch (msg.type) {
        case 'player_sync':
            // Update in-memory room state
            const room = rooms.get(client.roomCode);
            if (room) {
                const playerData = {
                    id: client.id,
                    name: client.name,
                    position: msg.position,
                    bodyYaw: msg.bodyYaw,
                    isSprinting: msg.isSprinting,
                    isAiming: msg.isAiming,
                    health: msg.health,
                    weapon: msg.weapon,
                    lastSeen: Date.now()
                };
                room.players.set(client.id, playerData);

                // Broadcast to all other players in this room
                broadcastToRoom(client.roomCode, {
                    type: 'remote_player_update',
                    player: playerData
                }, socket);
            }
            break;

        case 'player_fire':
            // Broadcast firing event for sound, muzzle flash, bullet trail
            broadcastToRoom(client.roomCode, {
                type: 'remote_player_fire',
                playerId: client.id,
                weapon: msg.weapon,
                origin: msg.origin,
                direction: msg.direction
            }, socket);
            break;

        case 'zombie_damage':
            // Synchronize damage dealt to infected zombies across clients
            broadcastToRoom(client.roomCode, {
                type: 'remote_zombie_damage',
                zombieId: msg.zombieId,
                damage: msg.damage,
                remainingHealth: msg.remainingHealth,
                isDead: msg.isDead
            }, socket);
            break;

        case 'ping':
            try {
                socket.write(encodeWebSocketFrame(JSON.stringify({ type: 'pong', clientTimestamp: msg.clientTimestamp, serverTimestamp: Date.now() })));
            } catch (e) {}
            break;
    }
}

function handleClientDisconnect(socket) {
    const client = connectedClients.get(socket);
    if (!client) return;

    connectedClients.delete(socket);

    const room = rooms.get(client.roomCode);
    if (room) {
        room.players.delete(client.id);
        // Announce departure to remaining players
        broadcastToRoom(client.roomCode, {
            type: 'player_disconnected',
            playerId: client.id
        });
    }

    console.log(`[WebSocket] Client disconnected: ${client.id} (Remaining: ${connectedClients.size})`);
}

server.listen(PORT, '0.0.0.0', () => {
    console.log(`==================================================================`);
    console.log(`  ECLIPSE: THE LAST CIVILIZATION — Master Game Server`);
    console.log(`  Version: 1.0.0-batchB | Engine: Unity 6 / WebGL`);
    console.log(`------------------------------------------------------------------`);
    console.log(`  > HTTP Game Client:   http://localhost:${PORT}`);
    console.log(`  > LAN / Network Link: http://0.0.0.0:${PORT}`);
    console.log(`  > WebSocket Bridge:   ws://localhost:${PORT}/ws`);
    console.log(`  > REST API Endpoint:  http://localhost:${PORT}/api/status`);
    console.log(`  > Save File Store:    ${SAVE_FILE}`);
    console.log(`==================================================================`);
});
