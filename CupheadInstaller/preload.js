'use strict';

const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('installer', {
  detectCuphead:    ()    => ipcRenderer.invoke('detect-cuphead'),
  browseFolder:     ()    => ipcRenderer.invoke('browse-folder'),
  checkInstallState: (d)  => ipcRenderer.invoke('check-install-state', d),
  startInstall:     (o)   => ipcRenderer.send('install', o),
  onInstallProgress: (cb) => ipcRenderer.on('install-progress', (_, d) => cb(d)),
  windowClose:      ()    => ipcRenderer.send('window-close'),
  windowMinimize:   ()    => ipcRenderer.send('window-minimize'),
});
