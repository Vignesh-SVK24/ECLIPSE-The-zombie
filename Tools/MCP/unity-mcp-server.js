#!/usr/bin/env node
/**
 * ECLIPSE: THE LAST CIVILIZATION
 * Unity MCP Bridge Server (Stdio to Localhost HTTP)
 * Connects Google Antigravity to active Unity 6 Editor via UnityMcpBridge.cs
 */

const http = require('http');
const readline = require('readline');

const UNITY_BRIDGE_HOST = '127.0.0.1';
const UNITY_BRIDGE_PORT = 8080;

// Read JSON-RPC lines from stdin
const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout,
  terminal: false
});

rl.on('line', async (line) => {
  if (!line || !line.trim()) return;

  let request;
  try {
    request = JSON.parse(line);
  } catch (err) {
    sendResponse(null, {
      jsonrpc: '2.0',
      error: { code: -32700, message: 'Parse error' }
    });
    return;
  }

  const { id, method, params } = request;

  if (method === 'initialize') {
    sendResponse(id, {
      protocolVersion: '2024-11-05',
      capabilities: {
        tools: {}
      },
      serverInfo: {
        name: 'unity-eclipse-mcp-bridge',
        version: '1.0.0'
      }
    });
    return;
  }

  if (method === 'tools/list') {
    sendResponse(id, {
      tools: [
        {
          name: 'unity_ping',
          description: 'Pings active Unity Editor, returns Unity version, active scene name, and playmode status.',
          inputSchema: { type: 'object', properties: {} }
        },
        {
          name: 'unity_get_scene_hierarchy',
          description: 'Retrieves complete GameObject hierarchy of the active Unity scene.',
          inputSchema: { type: 'object', properties: {} }
        },
        {
          name: 'unity_inspect_gameobject',
          description: 'Retrieves transform, components, colliders, and renderers for a GameObject by name.',
          inputSchema: {
            type: 'object',
            properties: {
              name: { type: 'string', description: 'Name of the GameObject in the scene' }
            },
            required: ['name']
          }
        },
        {
          name: 'unity_get_console_logs',
          description: 'Retrieves recent Unity Editor console messages, warnings, and errors.',
          inputSchema: { type: 'object', properties: {} }
        },
        {
          name: 'unity_generate_greyfall_scene',
          description: 'Executes automated generation of the photorealistic Greyfall City (Batch B) scene.',
          inputSchema: { type: 'object', properties: {} }
        },
        {
          name: 'unity_inspect_lighting',
          description: 'Inspects active scene directional light, ambient settings, and volumetric fog.',
          inputSchema: { type: 'object', properties: {} }
        },
        {
          name: 'unity_inspect_materials',
          description: 'Lists all unique PBR materials instantiated in the active scene.',
          inputSchema: { type: 'object', properties: {} }
        }
      ]
    });
    return;
  }

  if (method === 'tools/call') {
    const toolName = params.name;
    const args = params.arguments || {};

    let bridgeAction = 'ping';
    let queryParams = '';

    if (toolName === 'unity_ping') bridgeAction = 'ping';
    else if (toolName === 'unity_get_scene_hierarchy') bridgeAction = 'get_scene_hierarchy';
    else if (toolName === 'unity_inspect_gameobject') {
      bridgeAction = 'inspect_gameobject';
      queryParams = `&name=${encodeURIComponent(args.name || '')}`;
    }
    else if (toolName === 'unity_get_console_logs') bridgeAction = 'get_console_logs';
    else if (toolName === 'unity_generate_greyfall_scene') bridgeAction = 'generate_greyfall_scene';
    else if (toolName === 'unity_inspect_lighting') bridgeAction = 'inspect_lighting';
    else if (toolName === 'unity_inspect_materials') bridgeAction = 'inspect_materials';
    else {
      sendError(id, -32601, `Unknown tool: ${toolName}`);
      return;
    }

    try {
      const data = await queryUnityBridge(bridgeAction, queryParams);
      sendResponse(id, {
        content: [
          {
            type: 'text',
            text: typeof data === 'string' ? data : JSON.stringify(data, null, 2)
          }
        ]
      });
    } catch (err) {
      sendResponse(id, {
        content: [
          {
            type: 'text',
            text: JSON.stringify({
              status: 'offline',
              message: `Unity Editor Bridge not responding at http://${UNITY_BRIDGE_HOST}:${UNITY_BRIDGE_PORT}/mcp/. Ensure Unity 6 is open with the ECLIPSE project.`,
              error: err.message
            }, null, 2)
          }
        ],
        isError: true
      });
    }
    return;
  }

  sendError(id, -32601, `Method not found: ${method}`);
});

function queryUnityBridge(action, extraQuery = '') {
  return new Promise((resolve, reject) => {
    const url = `http://${UNITY_BRIDGE_HOST}:${UNITY_BRIDGE_PORT}/mcp/?action=${action}${extraQuery}`;
    http.get(url, (res) => {
      let rawData = '';
      res.on('data', (chunk) => { rawData += chunk; });
      res.on('end', () => {
        try {
          resolve(JSON.parse(rawData));
        } catch (e) {
          resolve(rawData);
        }
      });
    }).on('error', (err) => {
      reject(err);
    });
  });
}

function sendResponse(id, result) {
  const resp = { jsonrpc: '2.0', id, result };
  process.stdout.write(JSON.stringify(resp) + '\n');
}

function sendError(id, code, message) {
  const resp = { jsonrpc: '2.0', id, error: { code, message } };
  process.stdout.write(JSON.stringify(resp) + '\n');
}

