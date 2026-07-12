const proxyConfig = [
  {
    context: ['/api'],
    target: 'http://localhost:5260',
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