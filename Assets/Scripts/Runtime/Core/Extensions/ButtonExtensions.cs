using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.UI;

namespace Runtime.Core.Extensions
{
    public static class ButtonExtensions
    {
        #region Self

        public static void SubscribeButtonClick(this Button button, Subject<Unit> subject)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(Unit.Default))
                .AddTo(button);
        }

        public static void SubscribeButtonClick<T>(this Button button, Subject<T> subject, T parameter)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(parameter))
                .AddTo(button);
        }

        public static void SubscribeButtonClickAsync(this Button button, Func<UniTask> asyncAction)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction())
                .AddTo(button);
        }

        public static void SubscribeButtonClickAsync<T>(this Button button, Func<T, UniTask> asyncAction, T parameter)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction(parameter))
                .AddTo(button);
        }

        #endregion

        #region CompositeDisposable

        public static void SubscribeButtonClick(this Button button, Subject<Unit> subject, CompositeDisposable compositeDisposable)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(Unit.Default))
                .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClick<T>(this Button button, Subject<T> subject, T parameter, CompositeDisposable compositeDisposable)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(parameter))
                .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync(this Button button, Func<UniTask> asyncAction, CompositeDisposable compositeDisposable)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction())
                .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync<T>(this Button button, Func<T, UniTask> asyncAction, T parameter, CompositeDisposable compositeDisposable)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction(parameter))
                .AddTo(compositeDisposable);
        }

        #endregion

        #region CancellationToken

        public static void SubscribeButtonClickAsync(this Button button, Func<UniTask> asyncAction, CancellationToken cancellationToken)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction())
                .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClickAsync<T>(this Button button, Func<T, UniTask> asyncAction, T parameter, CancellationToken cancellationToken)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction(parameter))
                .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClickAsync(this Button button, Func<UniTask> asyncAction, CompositeDisposable compositeDisposable, CancellationToken cancellationToken)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction())
                .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClickAsync<T>(this Button button, Func<T, UniTask> asyncAction, T parameter, CompositeDisposable compositeDisposable, CancellationToken cancellationToken)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction(parameter))
                .AddTo(cancellationToken);
        }

        #endregion

        #region Additional Methods for Specific Types

        public static void SubscribeButtonClick(this Button button, Subject<int> subject, int parameter)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(parameter))
                .AddTo(button);
        }

        public static void SubscribeButtonClick(this Button button, Subject<int> subject, int parameter, CompositeDisposable compositeDisposable)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(parameter))
                .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync(this Button button, Func<int, UniTask> asyncAction, int parameter, CancellationToken cancellationToken)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction(parameter))
                .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClick(this Button button, Subject<string> subject, string parameter)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(parameter))
                .AddTo(button);
        }

        public static void SubscribeButtonClick(this Button button, Subject<string> subject, string parameter, CompositeDisposable compositeDisposable)
        {
            button
                .OnClickAsObservable()
                .Subscribe(_ => subject?.OnNext(parameter))
                .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync(this Button button, Func<string, UniTask> asyncAction, string parameter, CancellationToken cancellationToken)
        {
            button
                .OnClickAsObservable()
                .Subscribe(async _ => await asyncAction(parameter))
                .AddTo(cancellationToken);
        }

        #endregion
    }
}