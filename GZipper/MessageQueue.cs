using System;
using System.Collections.Generic;
using System.Threading;

namespace GZipper
{
    /// <summary>
    /// обработчик очереди.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения</typeparam>
    public abstract class MessageQueue<TMessage> : IDisposable
    {
        private readonly EventWaitHandle _event = new EventWaitHandle(false, EventResetMode.AutoReset);//Пришло новое сообщение

        private readonly Thread _eventThread;//Поток обработчика сообщения

        private readonly Queue<TMessage> _queue = new Queue<TMessage>( );//Очередь сообщений

        private volatile bool _stopped;//Признак конца обработки сообщений

        protected MessageQueue( )
        {
            _eventThread = new Thread(Run);
            _eventThread.Start( );
        }

        /// <summary>
        /// Запуск обработки
        /// </summary>
        private void Run( )
        {
            while (_event.WaitOne( ) && !_stopped)//Ждем когда придет сообщение
            {
                var cpy = new List<TMessage>();

                lock (_queue)//синхронизация доступа к очереди
                {
                    while (_queue.Count > 0)//обработка всех накопившихся сообщений
                        cpy.Add(_queue.Dequeue( ));
                }

                foreach (var item in cpy)
                    ProcessMessage(item);
            }

            // Обрабатываем то, что могло остаться необработанным на момент остановки
            lock (_queue)//синхронизация доступа к очереди
            {
                while (_queue.Count > 0)//обработка всех накопившихся сообщений
                    ProcessMessage(_queue.Dequeue( ));
            }
        }

        /// <summary>
        /// Ставим новое сообщение в очередь
        /// </summary>
        /// <param name="message"></param>
        public void Enqueue(TMessage message)
        {
            lock (_queue)
            {
                _queue.Enqueue(message);//Постановка в очередь
            }

            _event.Set( );//Оповещение фонового потока о событии
        }

        /// <summary>
        /// Остановка
        /// </summary>
        public void Stop( )
        {
            _stopped = true;

            _event.Set( );
            _eventThread.Join();
        }


        protected abstract void ProcessMessage(TMessage message);//Действие обрабатывающее сообщение

        /// <summary>
        /// Финализатор
        /// </summary>
        ~MessageQueue( )
        {
            Dispose(false);
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        public void Dispose( )
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_stopped)
                    this.Stop();

                _event.Dispose();
            }
        }
    }
}
