import fs from 'fs';
import { defineConfig } from 'vite';

export default defineConfig({
  server: {
    https: {
      key: fs.readFileSync('C:/Users/Vinni/AppData/Roaming/ASP.NET/https/rapidrabbitpackagecounter.key'),
      cert: fs.readFileSync('C:/Users/Vinni/AppData/Roaming/ASP.NET/https/rapidrabbitpackagecounter.pem')
    },
    port: 44475

  },
  build: {
    outDir: 'dist/browser',
  },
});
