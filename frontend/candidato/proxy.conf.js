const proxyConfig = [
  {
    context: ['/api'],
    target: 'http://localhost:3000/',
    secure: false,
    logLevel: 'debug'
  }
];

module.exports = proxyConfig;






/*
const proxyConfig = [
  {
    context: ['/api'],
    target: 'http://localhost:8080/',
    secure: false,
    logLevel: 'debug'
  }
];

module.exports = proxyConfig;
*/