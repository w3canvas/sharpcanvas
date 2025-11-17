namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Marker interface for objects that can be transferred between workers/contexts
    /// with zero-copy semantics. This includes ImageBitmap, OffscreenCanvas, ArrayBuffer, etc.
    ///
    /// When an object is transferred:
    /// - Ownership is moved to the destination
    /// - The original object becomes unusable (neutered)
    /// - No copying occurs (zero-copy transfer)
    /// </summary>
    public interface ITransferable
    {
        /// <summary>
        /// Checks if this object has been transferred and is no longer usable
        /// </summary>
        bool IsNeutered { get; }

        /// <summary>
        /// Marks this object as transferred/neutered
        /// </summary>
        void Neuter();
    }
}
