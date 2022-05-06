// JavaScript source code
mergeInto(LibraryManager.library, {
  WalletAddress: function () {
    var returnStr
    try {
      // get address from metamask
      returnStr = web3.currentProvider.selectedAddress
    } catch (e) {
      returnStr = ""
    }
    var returnStr = web3.currentProvider.selectedAddress;
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },
  getStakedCountAndAmount: async function (resultArray) {
                if (!window.ethereum) {
                    alert("Use a browser with MetaMask");
                    return;
                }

                const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
                const address = accounts[0];

                await window.ethereum.request({
                    method: "wallet_addEthereumChain",
                    params: [
                        {
                            chainId: "0x51",
                            chainName: "Shibuya",
                            nativeCurrency: {
                                name: "SBY",
                                symbol: "SBY",
                                decimals: 18,
                            },
                            rpcUrls: ["https://rpc.shibuya.astar.network:8545"],
                            blockExplorerUrls: ["https://shibuya.subscan.io/"],
                        },
                    ]
                });

                const contractAddress = "0x1A65738efdEf636D8533436d3Eb0d8ca1cf00Fa1"; // Stake contract (not NFT contract)
                const abi = [
                    {
                        "inputs": [
                            {
                                "internalType": "address",
                                "name": "",
                                "type": "address"
                            }
                        ],
                        "name": "stakerTotalAmount",
                        "outputs": [
                            {
                                "internalType": "uint256",
                                "name": "",
                                "type": "uint256"
                            }
                        ],
                        "stateMutability": "view",
                        "type": "function"
                    },
                    {
                        "inputs": [
                            {
                                "internalType": "address",
                                "name": "",
                                "type": "address"
                            }
                        ],
                        "name": "stakerTotalCount",
                        "outputs": [
                            {
                                "internalType": "uint256",
                                "name": "",
                                "type": "uint256"
                            }
                        ],
                        "stateMutability": "view",
                        "type": "function"
                    },
                ];
                const web3 = new Web3(window.ethereum);
                const contract = new web3.eth.Contract(abi, contractAddress);
                const count = await contract.methods.stakerTotalCount(address).call();
                const amount = await contract.methods.stakerTotalAmount(address).call();

                var returnStr = JSON.stringify({
                    count: Number(count.toString()),
                    amount: Number(amount.toString())
                });
                console.log({returnStr});

                var bufferSize = lengthBytesUTF8(returnStr) + 1;
                //var buffer = _malloc(bufferSize);
                stringToUTF8(returnStr, resultArray, bufferSize);
                //return buffer;
            },
  TestCopyToBuffer: function( array ){
    stringToUTF8("hogehoge", array, 9);
  },
  stake: async function (_projectAddress, _stakeAmount){
  if (!window.ethereum) {
                    alert("Use a browser with MetaMask");
                    return;
                }

                const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
                const address = accounts[0];

                await window.ethereum.request({
                    method: "wallet_addEthereumChain",
                    params: [
                        {
                            chainId: "0x51",
                            chainName: "Shibuya",
                            nativeCurrency: {
                                name: "SBY",
                                symbol: "SBY",
                                decimals: 18,
                            },
                            rpcUrls: ["https://rpc.shibuya.astar.network:8545"],
                            blockExplorerUrls: ["https://shibuya.subscan.io/"],
                        },
                    ]
                });

                const contractAddress = "0x1A65738efdEf636D8533436d3Eb0d8ca1cf00Fa1"; // Stake contract (not NFT contract)
                const abi = [
                    {
                        "inputs": [
                            {
                            "internalType": "address",
                            "name": "artistAddress",
                            "type": "address"
                            },
                            {
                            "internalType": "uint16",
                            "name": "_referralCode",
                            "type": "uint16"
                            }
                        ],
                        "name": "deposit",
                        "outputs": [],
                        "stateMutability": "payable",
                        "type": "function"
                    },
                ];

                // TODO:
                const projectAddress = Pointer_stringify(_projectAddress);
                const stakeAmount = Number(Pointer_stringify(_stakeAmount));
                //const projectAddress = _projectAddress;
                //const stakeAmount = _stakeAmount;

                const value = Web3.utils.toWei(String(stakeAmount), 'ether');
                const web3 = new Web3(window.ethereum);
                const contract = new web3.eth.Contract(abi, contractAddress);
                await contract.methods.deposit(projectAddress, 0).send({ value: value, from: address });
            }
});