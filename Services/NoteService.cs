﻿

namespace Booger
{
    using System.Threading;
    using System.Threading.Tasks;

    public class NoteService
    {
        public NoteDataModel Data { get; } = new NoteDataModel();

        private CancellationTokenSource cancellation;

        private async Task ShowCoreAsync(string msg, int timeout, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            Data.Text = msg;
            Data.Show = true;

            try
            {
                await Task.Delay(timeout, token);

                if (token.IsCancellationRequested)
                    return;

                Data.Show = false;
            }
            catch (TaskCanceledException) { }
        }

        public Task ShowAndWaitAsync(string msg, int timeout)
        {
            cancellation?.Cancel();
            cancellation = new CancellationTokenSource();

            return ShowCoreAsync(msg, timeout, cancellation.Token);
        }

        public void Show(string msg, int timeout)
        {
            cancellation?.Cancel();
            cancellation = new CancellationTokenSource();

            _ = ShowCoreAsync(msg, timeout, cancellation.Token);
        }

        public void Show(string msg)
        {
            Data.Text = msg;
            Data.Show = true;
        }

        public void Close()
        {
            cancellation?.Cancel();

            Data.Show = false;
        }
    }
}
