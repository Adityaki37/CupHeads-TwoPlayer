'use strict';

// Surface any JS errors as visible UI so they're not silent
window.onerror = (msg, src, line, col, err) => {
  const el = document.getElementById('global-error');
  if (el) { el.textContent = 'JS Error: ' + msg + ' (' + src + ':' + line + ')'; el.style.display = 'block'; }
  console.error('[app]', msg, err);
};

if (typeof installer === 'undefined') {
  console.error('[app] installer is undefined — preload may have failed');
}

let currentView  = 0;
let cupheadPath  = null;
let hasBepInEx   = false;

// ── Navigation ─────────────────────────────────────────────────────────────

function goTo(idx, direction = 1) {
  const views = document.querySelectorAll('.view');
  const active = document.querySelector('.view.active');

  if (active) {
    active.classList.remove('active');
    active.classList.add(direction >= 0 ? 'exit-left' : 'enter-left');
    setTimeout(() => active.classList.remove('exit-left', 'enter-left'), 350);
  }

  const next = views[idx];
  next.classList.add(direction >= 0 ? 'enter-right' : 'enter-left');
  void next.offsetWidth;                  // force reflow
  next.classList.remove('enter-right', 'enter-left');
  next.classList.add('active');

  document.querySelectorAll('.rail-step').forEach((d, i) => {
    d.classList.toggle('active', i === idx);
    d.classList.toggle('done',   i <  idx);
  });

  // Inked line fills up as the user advances
  const fill = document.querySelector('.rail-fill');
  if (fill) fill.style.setProperty('--progress', (idx / 3 * 100) + '%');

  currentView = idx;
  if (idx === 1) onEnterDetect();
  if (idx === 2) onEnterInstall();
}

// ── Title bar ──────────────────────────────────────────────────────────────

document.getElementById('btn-close').addEventListener('click',    () => installer.windowClose());
document.getElementById('btn-minimize').addEventListener('click', () => installer.windowMinimize());

// ── View 0 — Welcome ───────────────────────────────────────────────────────

document.getElementById('btn-start').addEventListener('click', () => goTo(1));

// ── View 1 — Detect ────────────────────────────────────────────────────────

async function onEnterDetect() {
  cupheadPath = null;
  hasBepInEx  = false;

  setDetect('scanning');
  disable('btn-d-next', true);

  try {
    const result = await installer.detectCuphead();
    if (result && result.path) {
      await applyPath(result.path);
    } else {
      setDetect('notfound');
    }
  } catch (e) {
    console.error('[detect]', e);
    setDetect('notfound');
  }
}

function setDetect(state) {
  set('detect-scanning', state === 'scanning');
  set('detect-notfound', state === 'notfound');
  set('detect-found',    state === 'found');
  set('badge-row',       state === 'found');
}

async function applyPath(dir) {
  setDetect('found');
  document.getElementById('detect-path').textContent = dir;

  const s = await installer.checkInstallState(dir);
  if (!s.valid) { setDetect('notfound'); return; }

  cupheadPath = dir;
  hasBepInEx  = !!s.hasBepInEx;

  // BepInEx badge
  const bepOk = s.hasBepInEx;
  badge('badge-bep', 'bdot-bep', 'bval-bep',
        bepOk, bepOk ? 'Installed' : 'Not found');

  // Plugin badge
  const plugOk = s.hasPlugin;
  badge('badge-plug', 'bdot-plug', 'bval-plug',
        plugOk, plugOk ? 'Up to date' : 'Not installed');

  disable('btn-d-next', false);
}

function badge(id, dotId, valId, ok, text) {
  const el = document.getElementById(id);
  el.className = 'status-badge ' + (ok ? 'ok' : 'nok');
  const vEl = document.getElementById(valId);
  if (vEl) vEl.textContent = text;
}

document.getElementById('btn-browse').addEventListener('click', async () => {
  const dir = await installer.browseFolder();
  if (!dir) return;
  setDetect('scanning');
  await applyPath(dir);
  if (!cupheadPath) setDetect('notfound');
});

document.getElementById('btn-change').addEventListener('click', async () => {
  const dir = await installer.browseFolder();
  if (!dir) return;
  await applyPath(dir);
});

document.getElementById('btn-d-back').addEventListener('click', () => goTo(0, -1));
document.getElementById('btn-d-next').addEventListener('click', () => goTo(2));

// ── View 2 — Install ───────────────────────────────────────────────────────

function onEnterInstall() {
  resetItem('ii-bep');
  resetItem('ii-plug');
  set('error-box', false);
  set('ii-bep-bar', false);
  fill('ii-bep-fill', 0);

  installer.startInstall({ cupheadDir: cupheadPath, skipBepInEx: hasBepInEx });
}

installer.onInstallProgress((data) => {
  const { type, step, status, progress, arch } = data;

  if (type === 'step') {
    if (step === 'bepinex') {
      if (status === 'downloading') {
        setItem('ii-bep', 'active',
          arch ? `Downloading ${arch}\u2026 ${progress}%` : `Downloading\u2026 ${progress}%`);
        set('ii-bep-bar', true);
        fill('ii-bep-fill', progress);
      } else if (status === 'extracting') {
        setItem('ii-bep', 'active', 'Extracting\u2026');
        fill('ii-bep-fill', 100);
      } else if (status === 'done') {
        setItem('ii-bep', 'done', 'Installed');
        set('ii-bep-bar', false);
        setGlyphCheck('ii-bep-ring');
      } else if (status === 'skipped') {
        setItem('ii-bep', 'skipped', 'Already installed');
        setGlyphCheck('ii-bep-ring');
      }
    }

    if (step === 'plugin') {
      if (status === 'installing') {
        setItem('ii-plug', 'active', 'Installing\u2026');
      } else if (status === 'done') {
        setItem('ii-plug', 'done', 'Installed');
        setGlyphCheck('ii-plug-ring');
      }
    }
  }

  if (type === 'done') {
    setTimeout(() => goTo(3), 700);
  }

  if (type === 'error') {
    set('error-box', true);
    document.getElementById('error-msg').textContent = data.message;
  }
});

// ── View 3 — Done ──────────────────────────────────────────────────────────

document.getElementById('btn-done').addEventListener('click', () => installer.windowClose());

// ── Helpers ────────────────────────────────────────────────────────────────

function set(id, visible) {
  const el = document.getElementById(id);
  if (!el) return;
  el.classList.toggle('hidden', !visible);
}

function disable(id, val) {
  const el = document.getElementById(id);
  if (el) el.disabled = val;
}

function fill(id, pct) {
  const el = document.getElementById(id);
  if (el) el.style.width = pct + '%';
}

function resetItem(id) {
  const el = document.getElementById(id);
  if (!el) return;
  el.className = 'install-item';
  const status = document.getElementById(id + '-status');
  if (status) status.textContent = 'Waiting';
  const ring = document.getElementById(id + '-ring');
  if (ring) {
    ring.classList.remove('done');
    ring.innerHTML =
      '<svg viewBox="0 0 32 32" fill="none">' +
      '<circle cx="16" cy="16" r="14" stroke="currentColor" stroke-width="1.5"/>' +
      '</svg>';
  }
}

function setItem(id, state, statusText) {
  const el = document.getElementById(id);
  if (!el) return;
  el.className = 'install-item ' + state;
  const s = document.getElementById(id + '-status');
  if (s && statusText) s.textContent = statusText;
}

function setGlyphCheck(ringId) {
  const el = document.getElementById(ringId);
  if (!el) return;
  el.classList.add('done');
  el.innerHTML =
    '<svg viewBox="0 0 32 32" fill="none">' +
    '<circle cx="16" cy="16" r="14" fill="var(--green-soft)" stroke="currentColor" stroke-width="1.5"/>' +
    '<path d="M10 16l4 4 8-8" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>' +
    '</svg>';
}
