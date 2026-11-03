const { execSync } = require('child_process');
const cwd = 'C:\\Users\\selva\\.gemini\\antigravity\\scratch\\eclipse-the-last-civilization';

console.log('1. Staging files with git add...');
execSync('git add .', { cwd, stdio: 'inherit' });

console.log('2. Committing files...');
execSync('git commit -m "Initial commit: ECLIPSE - THE LAST CIVILIZATION (Unity 6 + URP AAA Mobile Survival)"', { cwd, stdio: 'inherit' });

console.log('3. Pushing to origin main...');
execSync('git push -u origin main', { cwd, stdio: 'inherit' });

console.log('4. Push completed successfully!');
