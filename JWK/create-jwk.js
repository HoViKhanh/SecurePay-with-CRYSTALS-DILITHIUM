const jose = require('node-jose');

async function createJWK() {
    const keyStore = jose.JWK.createKeyStore();
    const key = await keyStore.generate("RSA", 2048, { alg: "RS256", use: "sig" });
    console.log(JSON.stringify(key.toJSON(), null, 2));
}

createJWK().catch(console.error);
