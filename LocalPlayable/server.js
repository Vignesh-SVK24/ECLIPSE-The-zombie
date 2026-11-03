const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = 5173;
const FILE_PATH = path.join(__dirname, 'index.html');

const server = http.createServer((req, res) => {
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

server.listen(PORT, '0.0.0.0', () => {
    console.log(`ECLIPSE: THE LAST CIVILIZATION is running locally at:`);
    console.log(`  > Local:   http://localhost:${PORT}`);
    console.log(`  > Network: http://0.0.0.0:${PORT}`);
});
