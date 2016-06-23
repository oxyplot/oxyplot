// --------------------------------------------------------------------------------------------------------------------
// <copyright file="D3D11Image.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// Stolen from https://github.com/sharpdx/Toolkit/blob/master/Source/Toolkit/SharpDX.Toolkit.Game/Desktop/D3D11Image.cs

namespace OxyPlot.SharpDX.WPF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Interop;
    using global::SharpDX.Direct3D11;
    using global::SharpDX.Direct3D9;
    using Device = global::SharpDX.Direct3D9.Device;
    using DXGIResource = global::SharpDX.DXGI.Resource;

    /// <summary>
    /// Represents the Direct3D11 Image.
    /// </summary>
    internal class D3D11Image : D3DImage, IDisposable
    {                
        /// <summary>
        /// The texture.
        /// </summary>
        private Texture texture;

        /// <summary>
        /// The texture surface handle
        /// </summary>
        private IntPtr textureSurfaceHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="D3D11Image"/> class. Associates an D3D11 render target with the current instance.
        /// </summary>
        /// <param name="device">A valid D3D9 DeviceEx.</param>
        /// <param name="renderTarget">A valid D3D11 render target. It must be created with the "Shared" flag.</param>
        public D3D11Image(DeviceEx device, Texture2D renderTarget)
        {
            using (var resource = renderTarget.QueryInterface<DXGIResource>())
            {              
                var handle = resource.SharedHandle;

                this.texture = new Texture(
                    device,
                    renderTarget.Description.Width,
                    renderTarget.Description.Height,
                    1,
                    Usage.RenderTarget,
                    Format.A8R8G8B8,
                    Pool.Default,
                    ref handle);
            }

            using (var surface = this.texture.GetSurfaceLevel(0))
            {
                this.textureSurfaceHandle = surface.NativePointer;
                this.TrySetBackbufferPointer(this.textureSurfaceHandle);
            }

            this.IsFrontBufferAvailableChanged += this.HandleIsFrontBufferAvailableChanged;
        }

        /// <summary>
        /// Marks the surface of element as invalid and requests its presentation on screen.
        /// </summary>
        public void InvalidateRendering()
        {
            if (this.texture == null)
            {
                return;
            }

            this.Lock();
            this.AddDirtyRect(new Int32Rect(0, 0, this.PixelWidth, this.PixelHeight));
            this.Unlock();
        }

        /// <summary>
        /// Try to set the back buffer pointer.
        /// </summary>
        /// <param name="ptr">A pointer to back buffer.</param>
        public void TrySetBackbufferPointer(IntPtr ptr)
        {
            // TODO: use TryLock and check multithreading scenarios
            this.Lock();
            try
            {
                this.SetBackBuffer(D3DResourceType.IDirect3DSurface9, ptr);
            }
            finally
            {
                this.Unlock();
            }
        }

        /// <summary>
        /// Disposes associated D3D9 texture.
        /// </summary>
        public void Dispose()
        {
            if (this.texture != null)
            {
                this.Dispatcher.BeginInvoke(
                    (Action)delegate
                    {
                        this.IsFrontBufferAvailableChanged -= this.HandleIsFrontBufferAvailableChanged;
                        this.texture.Dispose();
                        this.texture = null;
                        this.textureSurfaceHandle = IntPtr.Zero;

                        this.TrySetBackbufferPointer(IntPtr.Zero);
                    }, 
                    System.Windows.Threading.DispatcherPriority.Send);
            }
        }

        /// <summary>
        /// Front buffer change event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandleIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsFrontBufferAvailable)
            {
                this.TrySetBackbufferPointer(this.textureSurfaceHandle);
            }
        }
    }
}