using System;
using HDWallet.Core;
using HDWallet.Ed25519.Sample;
using NUnit.Framework;

namespace HDWallet.Polkadot.Tests
{
    public class GeneratePolkadotHDWallet
    {
        private const string mnemonic = "rapid apart clip require dragon property hurry ensure coil ship torch include squirrel jewel window";
        
        [Test]
        public void ShouldGenerateMasterWalletFromPurposeAndPath()
        {
            var testMne = "identify fatal close west parent myself awake impact shoot wide wrong derive ship doctor mushroom weather absent vacant armed chuckle swarm hip music wing";
            
            IHDWallet<SampleWallet> hdWallet = new TestHDWalletEd25519(testMne, "");
            var coinTypeWallet = hdWallet.GetAccount(0).GetExternalWallet(1); // "m/44'/354'/0'/0'/1'"
            var address = new PolkadotWallet(coinTypeWallet.PrivateKeyBytes.ToHexString()).GetNetworkAddress(AddressType.PolkadotLive);

            Console.WriteLine($"Address: {address}");
            Console.WriteLine($"Public key: {coinTypeWallet.PublicKeyBytes.ToHexString()}");
            Console.WriteLine($"Private key: {coinTypeWallet.PrivateKeyBytes.ToHexString()}");
        }
        
        [Test]
        public void ShouldGenerateFromMnemonic()
        {
            IHDWallet<PolkadotWallet> hdWallet = new PolkadotHDWallet(mnemonic,"");
            var seed = ((HdWalletBase)hdWallet).BIP39Seed;

            var wallet = hdWallet.GetAccount(0).GetExternalWallet(0);
            var address = wallet.Address;
            var polkadotAddress = wallet.GetNetworkAddress(AddressType.PolkadotLive);
            var publicKey = wallet.PublicKeyBytes;
            var privateKey = wallet.PrivateKeyBytes;
            var expandedPrivateKey = wallet.ExpandedPrivateKey;

            Assert.AreEqual(expected: "8bd78fe8b30abf91d3e9474c8927d9874fabc7e31ce2d866cf795378161f954a", actual: privateKey.ToHexString());
            Assert.AreEqual(expected: "8bd78fe8b30abf91d3e9474c8927d9874fabc7e31ce2d866cf795378161f954ab29b533725c02f6e69d8774c92d8a5a98506c2f09e13a1adbe4db367fbfa512a", actual: expandedPrivateKey.ToHexString());
            Assert.AreEqual(expected: "b29b533725c02f6e69d8774c92d8a5a98506c2f09e13a1adbe4db367fbfa512a", actual: publicKey.ToHexString());
            Assert.AreEqual(expected: "5G6tXDeie2KaUgGrwkBfzApKSCm9sE5QvBm1bi1vmBZ4gcCN", actual: address);
            Assert.AreEqual(expected: "153BfYunVob3vDHNuPEg8KeUHpkoZXdYzgVVm11HKGaarp6Y", actual: polkadotAddress);
            
            Console.WriteLine($"\nAddress: {address}");
            Console.WriteLine($"Public key: {publicKey.ToHexString()}");
            Console.WriteLine($"Private key: {privateKey.ToHexString()}");
            Console.WriteLine($"Expanded private key: {expandedPrivateKey.ToHexString()}");
        }

        [Test]
        public void ShouldGenerateKusamaFromMnemonic()
        {
            var kusamaMne = "identify fatal close west parent myself awake impact shoot wide wrong derive ship doctor mushroom weather absent vacant armed chuckle swarm hip music wing";
            IHDWallet<PolkadotWallet> hdWallet = new KusamaHDWallet(kusamaMne,"");

            var wallet = hdWallet.GetAccount(0).GetExternalWallet(0);
            var address = wallet.Address;

            Assert.AreEqual(expected: "5EkVRZntoGaMkRS4o9hFF6EWPgLFoP4vFSicNHqL9TFF6Sow", actual: address);
            Assert.AreEqual(expected: "13gnZu3xf3qqBxSaknkFPF4fFJKuVgd4KwT6XapghYGmH365", actual: wallet.GetNetworkAddress(AddressType.PolkadotLive));
        }

        [Test]
        public void ShouldGenerateFromSeed()
        {
            var seed = "ba78b733ffe929e400f844751a48dded5ebc7c62635a1590e97b066e3b9e8b890741602a69279c45ed5d17dfd6e8703e3c575de4ea4712868df5f1997e2b97b2";
            IHDWallet<PolkadotWallet> hdWallet = new PolkadotHDWallet(seed);

            var wallet = hdWallet.GetAccount(0).GetExternalWallet(0);
            var address = wallet.Address;
            var publicKey = wallet.PublicKeyBytes;
            var privateKey = wallet.PrivateKeyBytes;
            var expandedPrivateKey = wallet.ExpandedPrivateKey;

            Assert.AreEqual(expected: "8bd78fe8b30abf91d3e9474c8927d9874fabc7e31ce2d866cf795378161f954a", actual: privateKey.ToHexString());
            Assert.AreEqual(expected: "8bd78fe8b30abf91d3e9474c8927d9874fabc7e31ce2d866cf795378161f954ab29b533725c02f6e69d8774c92d8a5a98506c2f09e13a1adbe4db367fbfa512a", actual: expandedPrivateKey.ToHexString());
            Assert.AreEqual(expected: "b29b533725c02f6e69d8774c92d8a5a98506c2f09e13a1adbe4db367fbfa512a", actual: publicKey.ToHexString());
            Assert.AreEqual(expected: "5G6tXDeie2KaUgGrwkBfzApKSCm9sE5QvBm1bi1vmBZ4gcCN", actual: address);
        }

        [Test]
        public void ShouldGenerateAddressForMultipleChain()
        {
            var seed = "ba78b733ffe929e400f844751a48dded5ebc7c62635a1590e97b066e3b9e8b890741602a69279c45ed5d17dfd6e8703e3c575de4ea4712868df5f1997e2b97b2";
            IHDWallet<PolkadotWallet> hdWallet = new PolkadotHDWallet(seed);

            var wallet = hdWallet.GetAccount(0).GetExternalWallet(0);
            
            var genericNetworkAddress = wallet.Address;
            Assert.AreEqual(expected: "5G6tXDeie2KaUgGrwkBfzApKSCm9sE5QvBm1bi1vmBZ4gcCN", actual: genericNetworkAddress);

            var polkadotAddress = wallet.GetNetworkAddress(AddressType.PolkadotLive);
            Assert.AreEqual(expected: "153BfYunVob3vDHNuPEg8KeUHpkoZXdYzgVVm11HKGaarp6Y", actual: polkadotAddress);
        }

        IHDWallet<PolkadotWallet> hdWallet = new PolkadotHDWallet("ba78b733ffe929e400f844751a48dded5ebc7c62635a1590e97b066e3b9e8b890741602a69279c45ed5d17dfd6e8703e3c575de4ea4712868df5f1997e2b97b2");

        [TestCase(0, "5G6tXDeie2KaUgGrwkBfzApKSCm9sE5QvBm1bi1vmBZ4gcCN")]
        [TestCase(1, "5HYh1vCkCGa7gyPYnpcXbBqEtTgL2WQ7cMZjPmenLuQo6Kmw")]
        [TestCase(2, "5CAaEUL5YXPLRTSjPqB5JKJLaErdD7r8HaH6H7LPmgbsqfR9")]
        [TestCase(3, "5DGM7NVVLhqTcrXVtDbmUkXqxKK2TMLk2owPEHhKULb2vNcA")]
        [TestCase(4, "5CdedS14L983sVc7TwocWh1L2e2JPke8Mi1tSQsdsS4YZqsC")]
        [TestCase(100, "5DBnYvo59GcybxhTsQwTfTLhQKLKJU2ZGisxW2zRF11GdFaA")]
        [TestCase(1000, "5Gmq6VAK4LMmMG64tNSCxxUmTBCRbN6GvFcBfuEvTAeHT32m")]
        [TestCase(10000, "5D85aX1XMTyLUrGYK3wTFVypxJxSKgGQmnTjLEDKuk2ajbKX")]
        [TestCase(100000, "5GFPkx8fTfsS36EWmgENVEMuqvbDyu2juWh3abWXtwZeaMgK")]
        public void ShouldGenerateMultipleFromSeed(int index, string expectedAddress)
        {
            var wallet = hdWallet.GetAccount(0).GetExternalWallet((uint)index);

            Assert.AreEqual(expected: expectedAddress, actual: wallet.Address);
        }
    }
}