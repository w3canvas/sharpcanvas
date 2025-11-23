#!/usr/bin/env node

/**
 * Node.js test for SharpCanvas WASM
 * Tests loading and running SharpCanvas in Node.js environment
 */

import { createServer } from 'http';
import { readFileSync } from 'fs';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const wwwroot = join(__dirname, 'SharpCanvas.Blazor.Wasm/bin/Debug/net8.0/wwwroot');

console.log('🚀 SharpCanvas Node.js WASM Test\n');
console.log(`📁 Serving from: ${wwwroot}\n`);

// Simple HTTP server to serve WASM files
const server = createServer((req, res) => {
    let filePath = join(wwwroot, req.url === '/' ? 'index.html' : req.url);

    try {
        const content = readFileSync(filePath);

        // Set correct MIME types
        const ext = filePath.split('.').pop();
        const mimeTypes = {
            'wasm': 'application/wasm',
            'js': 'application/javascript',
            'json': 'application/json',
            'html': 'text/html',
            'css': 'text/css',
            'png': 'image/png',
            'svg': 'image/svg+xml'
        };

        res.writeHead(200, {
            'Content-Type': mimeTypes[ext] || 'text/plain',
            'Cross-Origin-Embedder-Policy': 'require-corp',
            'Cross-Origin-Opener-Policy': 'same-origin'
        });
        res.end(content);
    } catch (err) {
        res.writeHead(404);
        res.end('Not found');
    }
});

const PORT = 3000;
server.listen(PORT, () => {
    console.log(`✅ Server running on http://localhost:${PORT}`);
    console.log(`\n📖 Instructions:`);
    console.log(`   1. Open http://localhost:${PORT} in your browser`);
    console.log(`   2. Open DevTools Console to see SharpCanvas output`);
    console.log(`   3. Try the canvas demos`);
    console.log(`\n⏹️  Press Ctrl+C to stop\n`);
});

// Also test if we can load the WASM module directly
console.log('🔍 Checking WASM files...');
try {
    const wasmPath = join(wwwroot, '_framework/dotnet.native.wasm');
    const wasmBuffer = readFileSync(wasmPath);
    console.log(`✅ dotnet.native.wasm loaded: ${(wasmBuffer.length / 1024 / 1024).toFixed(2)} MB`);

    const contextPath = join(wwwroot, '_framework/Context.Skia.wasm');
    const contextBuffer = readFileSync(contextPath);
    console.log(`✅ Context.Skia.wasm loaded: ${(contextBuffer.length / 1024).toFixed(2)} MB`);

    console.log('✅ All WASM files accessible\n');
} catch (err) {
    console.error('❌ Error loading WASM files:', err.message);
}
