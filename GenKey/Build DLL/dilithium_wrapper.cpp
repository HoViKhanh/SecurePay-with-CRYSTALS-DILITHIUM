extern "C" {
#include <oqs/oqs.h>

OQS_SIG* oqs_sig_dilithium_new() {
    return OQS_SIG_new(OQS_SIG_alg_dilithium_5);
}

OQS_STATUS oqs_sig_dilithium_keypair(OQS_SIG* sig, uint8_t* public_key, uint8_t* secret_key) {
    return OQS_SIG_keypair(sig, public_key, secret_key);
}

OQS_STATUS oqs_sig_dilithium_sign(OQS_SIG* sig, uint8_t* signature, size_t* signature_len, const uint8_t* message, size_t message_len, const uint8_t* secret_key) {
    return OQS_SIG_sign(sig, signature, signature_len, message, message_len, secret_key);
}

OQS_STATUS oqs_sig_dilithium_verify(OQS_SIG* sig, const uint8_t* message, size_t message_len, const uint8_t* signature, size_t signature_len, const uint8_t* public_key) {
    return OQS_SIG_verify(sig, message, message_len, signature, signature_len, public_key);
}
}
