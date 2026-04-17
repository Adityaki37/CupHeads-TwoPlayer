'use strict';
// Copies the freshly-built CupheadOnline.dll into installer/assets/
// Run automatically before "npm run dist" via the "predist" hook.

const fs   = require('fs');
const path = require('path');

const DEST = path.resolve(__dirname, '..', 'assets', 'CupheadOnline.dll');
const candidates = [
  path.resolve(__dirname, '..', '..', 'CupheadOnline', 'bin', 'Release', 'net35', 'CupheadOnline.dll'),
  path.resolve(__dirname, '..', '..', 'CupheadOnline', 'bin', 'Release', 'net462', 'CupheadOnline.dll'),
  path.resolve(__dirname, '..', '..', 'CupheadOnline', 'bin', 'Debug', 'net35', 'CupheadOnline.dll'),
  path.resolve(__dirname, '..', '..', 'CupheadOnline', 'bin', 'Debug', 'net462', 'CupheadOnline.dll'),
];

const SRC = candidates.find((p) => fs.existsSync(p));
if (!SRC) {
  console.error('\n  ERROR: CupheadOnline.dll not found. Checked:');
  candidates.forEach((p) => console.error('    ' + p));
  console.error('  Build the mod first:');
  console.error('    dotnet build CupheadOnline/CupheadOnline.csproj -c Release\n');
  process.exit(1);
}

fs.mkdirSync(path.dirname(DEST), { recursive: true });
fs.copyFileSync(SRC, DEST);
console.log('  ✓ Copied CupheadOnline.dll → assets/');
