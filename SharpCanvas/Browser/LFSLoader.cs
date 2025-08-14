using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace SharpCanvas.Browser
{
    
    internal class LFSLoader: IFileLoader
    {
        private Uri _uri;
        private byte[] _content = null!;
        public event FileLoadedEventHandler? FileLoaded;

        public LFSLoader(Uri uri)
        {
            _uri = uri;
        }

        #region Implementation of IFileLoader

        public void BeginLoad()
        {
            string path = _uri.PathAndQuery;
            FileInfo fi = new FileInfo(path);
            if(fi.Exists)
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                // Create a synchronization object that gets 
                // signaled when read is complete.
                ManualResetEvent manualEvent = new ManualResetEvent(false);
                var stateObject = new State(fs, fi.Length, manualEvent);
                //todo: implement buffer of 4096 size and provide intermediate state notifications
                fs.BeginRead(stateObject.Buffer, 0, (int)stateObject.BufferSize, new AsyncCallback(EndReadCallback), stateObject);
            }
        }

        public byte[] Load()
        {
            string path = _uri.PathAndQuery;
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            throw new FileNotFoundException("File not found.", path);
        }

        // When BeginRead is finished reading data from the file, the 
        // EndReadCallback method is called to end the asynchronous 
        // read operation and fire appropriate event
        public void EndReadCallback(IAsyncResult asyncResult)
        {
            State? state = asyncResult.AsyncState as State;
            if (state == null) return;
            int readCount = state.FStream.EndRead(asyncResult);
            //store bytes inside class instance
            _content = state.Buffer;
            //close stream
            state.FStream.Close();
            // Signal the main thread that the read is finished.
            state.ManualEvent.Set();
            if(FileLoaded != null)
            {
                FileLoaded.Invoke(_content);
            }
        }


        #endregion
    }

    // Maintain state information to be passed to EndReadCallback.
    class State
    {
        // fStream is used to read from the file.
        FileStream _fStream;

        // data stores bytes that is read from the file.
        byte[] _buffer;
        
        // manualEvent signals the main thread 
        // when read is complete.
        ManualResetEvent manualEvent;

        private long _bufferSize;

        public State(FileStream fStream, long bufferSize, ManualResetEvent manualEvent)
        {
            this._fStream = fStream;
            this.manualEvent = manualEvent;
            _bufferSize = bufferSize;
            _buffer = new byte[bufferSize];
        }

        public FileStream FStream
        {
            get { return _fStream; }
        }

        public ManualResetEvent ManualEvent
        {
            get { return manualEvent; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public long BufferSize
        {
            get { return _bufferSize; }
        }
    }
}
