using System;
using System.Threading;
using Application.UI;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.UI;

namespace Runtime.Core.Extensions
{
    public static class SimpleButtonExtensions
    {
        #region Self

        public static void SubscribeButtonClick(this SimpleButton button, Subject<Unit> subject)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(Unit.Default))
                    .AddTo(button);
        }

        public static void SubscribeButtonClick<T>(this SimpleButton button, Subject<T> subject, T parameter)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(parameter))
                    .AddTo(button);
        }

        public static void SubscribeButtonClickAsync(this SimpleButton button, Func<UniTask> asyncAction)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction())
                    .AddTo(button);
        }

        public static void SubscribeButtonClickAsync<T>(this SimpleButton button, Func<T, UniTask> asyncAction, T parameter)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction(parameter))
                    .AddTo(button);
        }

        #endregion

        #region CompositeDisposable

        public static void SubscribeButtonClick(this SimpleButton button, Subject<Unit> subject, CompositeDisposable compositeDisposable)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(Unit.Default))
                    .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClick<T>(this SimpleButton button, Subject<T> subject, T parameter, CompositeDisposable compositeDisposable)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(parameter))
                    .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync(this SimpleButton button, Func<UniTask> asyncAction, CompositeDisposable compositeDisposable)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction())
                    .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync<T>(this SimpleButton button, Func<T, UniTask> asyncAction, T parameter, CompositeDisposable compositeDisposable)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction(parameter))
                    .AddTo(compositeDisposable);
        }

        #endregion

        #region CancellationToken

        public static void SubscribeButtonClickAsync(this SimpleButton button, Func<UniTask> asyncAction, CancellationToken cancellationToken)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction())
                    .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClickAsync<T>(this SimpleButton button, Func<T, UniTask> asyncAction, T parameter, CancellationToken cancellationToken)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction(parameter))
                    .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClickAsync(this SimpleButton button, Func<UniTask> asyncAction, CompositeDisposable compositeDisposable, CancellationToken cancellationToken)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction())
                    .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClickAsync<T>(this SimpleButton button, Func<T, UniTask> asyncAction, T parameter, CompositeDisposable compositeDisposable, CancellationToken cancellationToken)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction(parameter))
                    .AddTo(cancellationToken);
        }

        #endregion

        #region Additional Methods for Specific Types

        public static void SubscribeButtonClick(this SimpleButton button, Subject<int> subject, int parameter)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(parameter))
                    .AddTo(button);
        }

        public static void SubscribeButtonClick(this SimpleButton button, Subject<int> subject, int parameter, CompositeDisposable compositeDisposable)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(parameter))
                    .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync(this SimpleButton button, Func<int, UniTask> asyncAction, int parameter, CancellationToken cancellationToken)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction(parameter))
                    .AddTo(cancellationToken);
        }

        public static void SubscribeButtonClick(this SimpleButton button, Subject<string> subject, string parameter)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(parameter))
                    .AddTo(button);
        }

        public static void SubscribeButtonClick(this SimpleButton button, Subject<string> subject, string parameter, CompositeDisposable compositeDisposable)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => subject?.OnNext(parameter))
                    .AddTo(compositeDisposable);
        }

        public static void SubscribeButtonClickAsync(this SimpleButton button, Func<string, UniTask> asyncAction, string parameter, CancellationToken cancellationToken)
        {
            button.Button
                    .OnClickAsObservable()
                    .Subscribe(async _ => await asyncAction(parameter))
                    .AddTo(cancellationToken);
        }

        #endregion
    }
}