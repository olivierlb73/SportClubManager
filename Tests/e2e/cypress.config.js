const { defineConfig } = require('cypress')

module.exports = defineConfig({
    e2e: {
        baseUrl: 'http://localhost:5035',
        supportFile: false,
        allowCypressEnv: false,
    },
})