﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace GamePlannerWeb
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// Extension methods for EventRegistrations.
    /// </summary>
    public static partial class EventRegistrationsExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<EventRegistration> GetEventRegistrations(this IEventRegistrations operations)
            {
                return Task.Factory.StartNew(s => ((IEventRegistrations)s).GetEventRegistrationsAsync(), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<EventRegistration>> GetEventRegistrationsAsync(this IEventRegistrations operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetEventRegistrationsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='eventRegistration'>
            /// </param>
            public static EventRegistration PostEventRegistration(this IEventRegistrations operations, EventRegistration eventRegistration)
            {
                return Task.Factory.StartNew(s => ((IEventRegistrations)s).PostEventRegistrationAsync(eventRegistration), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='eventRegistration'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<EventRegistration> PostEventRegistrationAsync(this IEventRegistrations operations, EventRegistration eventRegistration, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostEventRegistrationWithHttpMessagesAsync(eventRegistration, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static EventRegistration GetEventRegistration(this IEventRegistrations operations, int id)
            {
                return Task.Factory.StartNew(s => ((IEventRegistrations)s).GetEventRegistrationAsync(id), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<EventRegistration> GetEventRegistrationAsync(this IEventRegistrations operations, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetEventRegistrationWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='eventRegistration'>
            /// </param>
            public static void PutEventRegistration(this IEventRegistrations operations, int id, EventRegistration eventRegistration)
            {
                Task.Factory.StartNew(s => ((IEventRegistrations)s).PutEventRegistrationAsync(id, eventRegistration), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='eventRegistration'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task PutEventRegistrationAsync(this IEventRegistrations operations, int id, EventRegistration eventRegistration, CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.PutEventRegistrationWithHttpMessagesAsync(id, eventRegistration, null, cancellationToken).ConfigureAwait(false);
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static EventRegistration DeleteEventRegistration(this IEventRegistrations operations, int id)
            {
                return Task.Factory.StartNew(s => ((IEventRegistrations)s).DeleteEventRegistrationAsync(id), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<EventRegistration> DeleteEventRegistrationAsync(this IEventRegistrations operations, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteEventRegistrationWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
