using System;

using MetaMask.Models;

using UnityEngine;
using UnityEngine.UI;

using ZXing;
using ZXing.QrCode;

namespace MetaMask.Transports.Unity.UI
{

    public class MetaMaskUnityUIQRImage : MonoBehaviour, IMetaMaskUnityTransportListener
    {

        /// <summary>The raw image to display.</summary>
        [SerializeField]
        protected RawImage rawImage;

        /// <summary>Resets the image to its original state.</summary>
        private void Reset()
        {
            this.rawImage = GetComponent<RawImage>();
        }

        /// <summary>Called when the connection is being established.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MetaMaskUnityConnectEventArgs"/> instance containing the event data.</param>
        private void OnConnecting(object sender, MetaMaskUnityConnectEventArgs e)
        {
            ShowQR(e.Url);
        }

        /// <summary>Shows a QR code for the specified URL.</summary>       
        /// <param name="url">The URL to show in the QR code.</param>       
        public void ShowQR(string url)
        {
            this.rawImage.texture = GenerateQRTexture(url);
        }

        /// <summary>Encodes the given text into a QR code.</summary>
        /// <param name="textForEncoding">The text to encode.</param>
        /// <param name="width">The width of the QR code.</param>
        /// <param name="height">The height of the QR code.</param>
        /// <returns>The QR code as a 2D array of colors.</returns>
        private static Color32[] EncodeToQR(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }

        /// <summary>Generates a QR code texture.</summary>
        /// <param name="text">The text to encode.</param>
        /// <returns>The QR code texture.</returns>
        private static Texture2D GenerateQRTexture(string text)
        {
            Texture2D encoded = new Texture2D(256, 256);
            var color32 = EncodeToQR(text, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            return encoded;
        }

        public void OnMetaMaskConnectRequest(string url)
        {
            ShowQR(url);
        }

        public void OnMetaMaskRequest(string id, MetaMaskEthereumRequest request)
        {
        }

        public void OnMetaMaskFailure(Exception error)
        {
        }

        public void OnMetaMaskSuccess()
        {
        }
    }

}