using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Truffle.Functions;
using Truffle.Data;

namespace Truffle.Contracts
{
    public partial class TicTacToeService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TicTacToeDeployment ticTacToeDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TicTacToeDeployment>().SendRequestAndWaitForReceiptAsync(ticTacToeDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TicTacToeDeployment ticTacToeDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TicTacToeDeployment>().SendRequestAsync(ticTacToeDeployment);
        }

        public static async Task<TicTacToeService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TicTacToeDeployment ticTacToeDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, ticTacToeDeployment, cancellationTokenSource);
            return new TicTacToeService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TicTacToeService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public TicTacToeService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> PaymentsQueryAsync(PaymentsFunction paymentsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PaymentsFunction, BigInteger>(paymentsFunction, blockParameter);
        }

        
        public Task<BigInteger> PaymentsQueryAsync(string dest, BlockParameter blockParameter = null)
        {
            var paymentsFunction = new PaymentsFunction();
                paymentsFunction.Dest = dest;
            
            return ContractHandler.QueryAsync<PaymentsFunction, BigInteger>(paymentsFunction, blockParameter);
        }

        public Task<string> WithdrawPaymentsRequestAsync(WithdrawPaymentsFunction withdrawPaymentsFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawPaymentsFunction);
        }

        public Task<TransactionReceipt> WithdrawPaymentsRequestAndWaitForReceiptAsync(WithdrawPaymentsFunction withdrawPaymentsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawPaymentsFunction, cancellationToken);
        }

        public Task<string> WithdrawPaymentsRequestAsync(string payee)
        {
            var withdrawPaymentsFunction = new WithdrawPaymentsFunction();
                withdrawPaymentsFunction.Payee = payee;
            
             return ContractHandler.SendRequestAsync(withdrawPaymentsFunction);
        }

        public Task<TransactionReceipt> WithdrawPaymentsRequestAndWaitForReceiptAsync(string payee, CancellationTokenSource cancellationToken = null)
        {
            var withdrawPaymentsFunction = new WithdrawPaymentsFunction();
                withdrawPaymentsFunction.Payee = payee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawPaymentsFunction, cancellationToken);
        }

        public Task<string> StartGameRequestAsync(StartGameFunction startGameFunction)
        {
             return ContractHandler.SendRequestAsync(startGameFunction);
        }

        public Task<TransactionReceipt> StartGameRequestAndWaitForReceiptAsync(StartGameFunction startGameFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startGameFunction, cancellationToken);
        }

        public Task<string> StartGameRequestAsync(string payoutX, string payoutO)
        {
            var startGameFunction = new StartGameFunction();
                startGameFunction.PayoutX = payoutX;
                startGameFunction.PayoutO = payoutO;
            
             return ContractHandler.SendRequestAsync(startGameFunction);
        }

        public Task<TransactionReceipt> StartGameRequestAndWaitForReceiptAsync(string payoutX, string payoutO, CancellationTokenSource cancellationToken = null)
        {
            var startGameFunction = new StartGameFunction();
                startGameFunction.PayoutX = payoutX;
                startGameFunction.PayoutO = payoutO;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startGameFunction, cancellationToken);
        }

        public Task<string> EndGameRequestAsync(EndGameFunction endGameFunction)
        {
             return ContractHandler.SendRequestAsync(endGameFunction);
        }

        public Task<TransactionReceipt> EndGameRequestAndWaitForReceiptAsync(EndGameFunction endGameFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(endGameFunction, cancellationToken);
        }

        public Task<string> EndGameRequestAsync(BigInteger gameId, BigInteger winner)
        {
            var endGameFunction = new EndGameFunction();
                endGameFunction.GameId = gameId;
                endGameFunction.Winner = winner;
            
             return ContractHandler.SendRequestAsync(endGameFunction);
        }

        public Task<TransactionReceipt> EndGameRequestAndWaitForReceiptAsync(BigInteger gameId, BigInteger winner, CancellationTokenSource cancellationToken = null)
        {
            var endGameFunction = new EndGameFunction();
                endGameFunction.GameId = gameId;
                endGameFunction.Winner = winner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(endGameFunction, cancellationToken);
        }
    }
}
