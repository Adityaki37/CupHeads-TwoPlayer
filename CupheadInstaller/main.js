'use strict';

const { app, BrowserWindow, ipcMain, dialog } = require('electron');
const path   = require('path');
const fs     = require('fs');
const https  = require('https');
const http   = require('http');
const os     = require('os');
const { execSync } = require('child_process');

let mainWindow;

// ── Window ─────────────────────────────────────────────────────────────────

function createWindow() {
  mainWindow = new BrowserWindow({
    width:           880,
    height:          620,
    frame:           false,
    resizable:       false,
    backgroundColor: '#080808',
    webPreferences: {
      preload:          path.join(__dirname, 'preload.js'),
      contextIsolation: true,
      nodeIntegration:  false,
      sandbox:          false,   // ensure preload has require() access
    },
  });

  mainWindow.loadFile(path.join(__dirname, 'renderer', 'index.html'));

  // F12 toggles DevTools for debugging
  mainWindow.webContents.on('before-input-event', (_, input) => {
    if (input.key === 'F12' && input.type === 'keyDown')
      mainWindow.webContents.toggleDevTools();
  });

  mainWindow.webContents.on('did-fail-load', (_, code, desc) => {
    console.error('[main] Page failed to load:', code, desc);
  });

  mainWindow.webContents.on('render-process-gone', (_, details) => {
    console.error('[main] Renderer crashed:', details.reason);
  });
}

app.whenReady().then(createWindow);
app.on('window-all-closed', () => app.quit());

// ── Window controls ────────────────────────────────────────────────────────

ipcMain.on('window-close',    () => app.quit());
ipcMain.on('window-minimize', () => mainWindow && mainWindow.minimize());

// ── Steam / Cuphead detection ──────────────────────────────────────────────

function getSteamPath() {
  // Registry (most reliable on Windows)
  const keys = [
    'HKLM\\SOFTWARE\\WOW6432Node\\Valve\\Steam',
    'HKLM\\SOFTWARE\\Valve\\Steam',
    'HKCU\\SOFTWARE\\Valve\\Steam',
  ];
  for (const key of keys) {
    try {
      const out = execSync(`reg query "${key}" /v InstallPath`, { encoding: 'utf8', timeout: 3000 });
      const m = out.match(/InstallPath\s+REG_SZ\s+(.+)/);
      if (m) return m[1].trim();
    } catch { /* continue */ }
  }
  // Fallback: common install locations
  for (const p of [
    'C:\\Program Files (x86)\\Steam',
    'C:\\Program Files\\Steam',
    path.join(os.homedir(), 'Steam'),
  ]) {
    if (fs.existsSync(path.join(p, 'steam.exe'))) return p;
  }
  return null;
}

function parseVdfLibraryPaths(content) {
  const paths = [];
  // Match every "path" "..." entry in the file
  const re = /"path"\s+"([^"]+)"/gi;
  let m;
  while ((m = re.exec(content)) !== null) {
    // VDF double-escapes backslashes: \\ → \
    paths.push(m[1].replace(/\\\\/g, '\\'));
  }
  return paths;
}

function findCuphead() {
  const steamPath = getSteamPath();
  const candidates = steamPath ? [steamPath] : [];

  if (steamPath) {
    const vdf = path.join(steamPath, 'steamapps', 'libraryfolders.vdf');
    if (fs.existsSync(vdf)) {
      const extra = parseVdfLibraryPaths(fs.readFileSync(vdf, 'utf8'));
      candidates.push(...extra);
    }
  }

  for (const lib of candidates) {
    const dir = path.join(lib, 'steamapps', 'common', 'Cuphead');
    if (fs.existsSync(path.join(dir, 'Cuphead.exe'))) return dir;
  }
  return null;
}

ipcMain.handle('detect-cuphead', () => {
  try { return { path: findCuphead(), error: null }; }
  catch (e) { return { path: null, error: e.message }; }
});

ipcMain.handle('browse-folder', async () => {
  const r = await dialog.showOpenDialog(mainWindow, {
    properties: ['openDirectory'],
    title: 'Select Cuphead Installation Folder',
  });
  return r.canceled ? null : r.filePaths[0];
});

ipcMain.handle('check-install-state', (_, dir) => {
  if (!dir || !fs.existsSync(dir)) return { valid: false };
  return {
    valid:      fs.existsSync(path.join(dir, 'Cuphead.exe')),
    hasBepInEx: fs.existsSync(path.join(dir, 'BepInEx')),
    hasPlugin:  fs.existsSync(path.join(dir, 'BepInEx', 'plugins', 'CupheadOnline', 'CupheadOnline.dll')),
  };
});

// ── Architecture detection (PE header) ────────────────────────────────────

function getCupheadArch(cupheadDir) {
  try {
    const exe = path.join(cupheadDir, 'Cuphead.exe');
    const buf = Buffer.alloc(0x200);
    const fd  = fs.openSync(exe, 'r');
    fs.readSync(fd, buf, 0, 0x200, 0);
    fs.closeSync(fd);
    if (buf.readUInt16LE(0) !== 0x5A4D) return 'x64'; // no MZ
    const peOff = buf.readUInt32LE(0x3C);
    if (peOff + 6 > buf.length)            return 'x64';
    if (buf.readUInt32LE(peOff) !== 0x4550) return 'x64'; // no PE\0\0
    const machine = buf.readUInt16LE(peOff + 4);
    return machine === 0x014C ? 'x86' : 'x64'; // 0x014C = i386
  } catch { return 'x64'; }
}

// ── Download with redirect + progress ─────────────────────────────────────

function download(url, dest, onProgress) {
  return new Promise((resolve, reject) => {
    const file = fs.createWriteStream(dest);
    let hops = 0;

    function doGet(reqUrl) {
      const mod = reqUrl.startsWith('https') ? https : http;
      const req = mod.get(reqUrl, (res) => {
        if (res.statusCode >= 300 && res.statusCode < 400 && res.headers.location) {
          if (++hops > 10) { reject(new Error('Too many redirects')); return; }
          doGet(res.headers.location);
          return;
        }
        if (res.statusCode !== 200) {
          reject(new Error('Download failed (HTTP ' + res.statusCode + ')'));
          return;
        }
        const total = parseInt(res.headers['content-length'] || '0', 10);
        let recv = 0;
        res.on('data', (chunk) => {
          recv += chunk.length;
          if (total > 0 && onProgress) onProgress(Math.round(recv / total * 100));
        });
        res.pipe(file);
        file.on('finish', () => file.close(resolve));
        res.on('error',   (e) => file.close(() => reject(e)));
      });
      req.on('error', (e) => file.close(() => reject(e)));
    }

    doGet(url);
  });
}

// ── Install handler ────────────────────────────────────────────────────────

const BEPINEX = {
  x64: 'https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.2/BepInEx_x64_5.4.23.2.zip',
  x86: 'https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.2/BepInEx_x86_5.4.23.2.zip',
};

function fetchJson(url) {
  return new Promise((resolve, reject) => {
    const req = https.get(url, {
      headers: { 'User-Agent': 'CupheadOnline-Installer' },
    }, (res) => {
      let body = '';
      res.on('data', (chunk) => body += chunk);
      res.on('end', () => {
        if (res.statusCode >= 200 && res.statusCode < 300) {
          try { resolve(JSON.parse(body)); }
          catch (err) { reject(err); }
        } else {
          reject(new Error(`HTTP ${res.statusCode}: ${url}`));
        }
      });
    });
    req.on('error', reject);
  });
}

async function getLatestBepInExUrl(arch) {
  try {
    const release = await fetchJson('https://api.github.com/repos/BepInEx/BepInEx/releases/latest');
    if (!release || !Array.isArray(release.assets)) throw new Error('Invalid release data');
    const assetRE = arch === 'x64'
      ? /BepInEx_x64_.*\.zip$/i
      : /BepInEx_x86_.*\.zip$/i;
    const asset = release.assets.find((a) => assetRE.test(a.name));
    if (asset && asset.browser_download_url) return asset.browser_download_url;
  } catch (err) {
    console.warn('[main] failed to resolve latest BepInEx release:', err.message);
  }
  return BEPINEX[arch];
}

ipcMain.on('install', async (event, { cupheadDir, skipBepInEx }) => {
  const send = (type, data) => {
    try {
      if (!event.sender.isDestroyed())
        event.sender.send('install-progress', Object.assign({ type }, data || {}));
    } catch { /* window closed */ }
  };

  try {
    // ── Step 1: BepInEx ──────────────────────────────────────────────────
    if (!skipBepInEx) {
      const arch    = getCupheadArch(cupheadDir);
      const zipUrl  = await getLatestBepInExUrl(arch);
      const zipPath = path.join(os.tmpdir(), path.basename(zipUrl));

      send('step', { step: 'bepinex', status: 'downloading', progress: 0, arch, url: zipUrl });

      await download(zipUrl, zipPath, (pct) =>
        send('step', { step: 'bepinex', status: 'downloading', progress: pct, arch }));

      send('step', { step: 'bepinex', status: 'extracting', progress: 100 });

      const AdmZip = require('adm-zip');
      new AdmZip(zipPath).extractAllTo(cupheadDir, /*overwrite*/ true);
      try { fs.unlinkSync(zipPath); } catch { /* best-effort */ }

      send('step', { step: 'bepinex', status: 'done', progress: 100 });
    } else {
      send('step', { step: 'bepinex', status: 'skipped' });
    }

    // ── Step 2: CupheadOnline DLL ────────────────────────────────────────
    send('step', { step: 'plugin', status: 'installing' });

    const pluginsDir = path.join(cupheadDir, 'BepInEx', 'plugins', 'CupheadOnline');
    fs.mkdirSync(pluginsDir, { recursive: true });

    const dllSrc = app.isPackaged
      ? path.join(process.resourcesPath, 'CupheadOnline.dll')
      : path.join(__dirname, 'assets', 'CupheadOnline.dll');

    if (!fs.existsSync(dllSrc))
      throw new Error('CupheadOnline.dll not found in installer package.\n' +
                      'Run "npm run predist" to copy the DLL first.');

    fs.copyFileSync(dllSrc, path.join(pluginsDir, 'CupheadOnline.dll'));
    send('step', { step: 'plugin', status: 'done' });

    // ── All done ──────────────────────────────────────────────────────────
    send('done', {});

  } catch (err) {
    send('error', { message: err.message });
  }
});
