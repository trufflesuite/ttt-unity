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
    public partial class PullPaymentService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, PullPaymentDeployment pullPaymentDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<PullPaymentDeployment>().SendRequestAndWaitForReceiptAsync(pullPaymentDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, PullPaymentDeployment pullPaymentDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<PullPaymentDeployment>().SendRequestAsync(pullPaymentDeployment);
        }

        public static async Task<PullPaymentService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, PullPaymentDeployment pullPaymentDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, pullPaymentDeployment, cancellationTokenSource);
            return new PullPaymentService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public PullPaymentService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public PullPaymentService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
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
    }
}
