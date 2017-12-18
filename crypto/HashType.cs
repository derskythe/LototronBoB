using System;
using System.Collections.Generic;
using System.Text;

namespace crypto
{
    public enum HashType
    {
        SHA1withDSA, //-- DSA
        SHA1withECDSA, //
        SHA224withECDSA, // ECDSA with SHA1 and SHA2 support
        SHA256withECDSA, //
        SHA384withECDSA, //
        SHA512withECDSA, //
        MD2withRSA, // --
        MD5withRSA, // --
        SHA1withRSA, // --
        SHA224withRSA, // -- RSA with MD2, MD5, SHA1, SHA2 and RIPEMD
        SHA256withRSA, // --
        SHA384withRSA, // --
        SHA512withRSA, // --
        RIPEMD160withRSA, // -- RIPEMD hash
        RIPEMD128withRSA, // --
        RIPEMD256withRSA, // --
    }
}
