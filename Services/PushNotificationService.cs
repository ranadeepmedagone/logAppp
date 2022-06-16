// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using FirebaseAdmin;
// using FirebaseAdmin.Messaging;
// using Google.Apis.Auth.OAuth2;
// using logapp.Entities;

// namespace logapp.Services
// {
//     public interface IPushNotificationService
//     {
//         // public Task SendPushNotification(IEnumerable<PushNotificationData> data);

//         /// <summary>
//         /// Send push notification to multiple tokens. Will be sent in batches of 500
//         /// </summary>
//         // Task SendMulticast(List<string> tokens, PushNotificationData data);
//         // Task SendSingle(PushNotificationData data, bool StoreInDb = false);

//         /// <summary>
//         /// Send personalised push notification with string interpolation based on data to all tokens
//         /// </summary>
//         // Task SendAll(List<PushNotificationData> data);
//         Task SendToAllFillUserNumber(PushNotificationData data, List<LoginStatIdWithUserId> UserIds);
//     }

//     public class FirebasePushNotificationService : IPushNotificationService
//     {
//         private readonly IConfiguration _config;
//         // private readonly ICachingService _cache;
//         // private readonly IBackgroundJobClient _bgJob;
//         private readonly ILogger<FirebasePushNotificationService> _logger;

//         ///https://samwalpole.com/using-scoped-services-inside-singletons
//         private readonly IServiceScopeFactory _serviceScopeFactory;

//         private readonly int batchCount = 100;
//         private readonly int networkRequestParallelBatches = 5;
//         private bool isDevelopment = true;

//         // public FirebasePushNotificationService(
//             IConfiguration config,
//             // ICachingService cache,
//             // IBackgroundJobClient bgJob,
//             ILogger<FirebasePushNotificationService> logger,
//             IServiceScopeFactory serviceScopeFactory
//             )
//         {
//             // this._cache = cache;
//             // this._config = config;
//             // this._bgJob = bgJob;
//             this._logger = logger;
//             _serviceScopeFactory = serviceScopeFactory;
//             isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

//             string startupPath = System.IO.Directory.GetCurrentDirectory();
//             var path = isDevelopment ? "/firebase.dev.json" : "/firebase.json";
//             using (StreamReader r = new StreamReader(startupPath + path))
//             {
//                 string json = r.ReadToEnd();
//                 FirebaseApp.Create(new AppOptions()
//                 {
//                     Credential = GoogleCredential.FromJson(json)
//                 });
//             }
//         }

//         // private async Task<bool> GetSetting()
//         // {
//         //     var settings = await _cache.GetSettings();
//         //     return bool.Parse(settings.EnablePushNotifications);
//         // }

//         // private async Task StoreInSyncQueue(List<PushNotificationData> data)
//         // {
//         //     try
//         //     {
//         //         using (var scope = _serviceScopeFactory.CreateScope())
//         //         {
//         //             var myScopedService = scope.ServiceProvider.GetService<INotificationRepository>();
//         //             await myScopedService.InsertPushNotificationInSyncQueue(data);
//         //             // await _notify.Value.InsertPushNotificationForUser(data);
//         //         }
//         //     }
//         //     catch (Exception e)
//         //     {
//         //         _logger.LogError("Error in scoped service resolution - FirebasePushNotificationService");
//         //         _logger.LogError(e.ToString());
//         //     }
//         // }

//         // public async Task DoMulticast(List<string> tokens, PushNotificationData data)
//         // {
//         //     if (!(await GetSetting()))
//         //         return;

//         //     var message = new MulticastMessage()
//         //     {
//         //         Tokens = tokens,
//         //         Data = data.Data,
//         //         // Android = new AndroidConfig() {}
//         //         Notification = new FirebaseAdmin.Messaging.Notification()
//         //         {
//         //             Title = data.TitleText,
//         //             Body = data.BodyText,
//         //             ImageUrl = data.MediaUrl,
//         //         },
//         //     };

//         //     var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
//         // }

//         // public async Task SendMulticast(List<string> tokens, PushNotificationData data)
//         // {
//         //     if (!(await GetSetting()))
//         //         return;

//         //     if (tokens is null || tokens.Count == 0)
//         //         return;

//         //     if (tokens.Count > batchCount)
//         //     {
//         //         var noOfBatches = Math.Ceiling(tokens.Count / (double)batchCount);
//         //         for (var i = 0; i < noOfBatches; i++)
//         //         {
//         //             _bgJob.Enqueue(() => DoMulticast(tokens.Skip(batchCount * i).Take(batchCount).ToList(), data));
//         //         }
//         //     }
//         //     else
//         //     {
//         //         await DoMulticast(tokens, data);
//         //     }
//         // }

//         public async Task SendSingle(PushNotificationData data, bool StoreInDb = false)
//         {
//             var message = new Message()
//             {
//                 Token = data.NotificationToken,
//                 Data = data.MergedData,
//                 Notification = new FirebaseAdmin.Messaging.Notification()
//                 {
//                     Title = data.TitleText,
//                     Body = data.BodyContentFormattedText,
//                     ImageUrl = data.MediaUrl,
//                 },
//             };

//             string res = null;
//             // if (await GetSetting() && !string.IsNullOrWhiteSpace(data.NotificationToken))
//             //     res = await FirebaseMessaging.DefaultInstance.SendAsync(message);

//             // if (StoreInDb)
//             //     await StoreInSyncQueue(new List<PushNotificationData> { data });
//         }

//         public async Task SendAll(List<PushNotificationData> data)
//         {
//             if (!(await GetSetting()))
//                 return;

//             if (data is null || data.Count == 0)
//                 return;

//             _logger.LogInformation("Sending SendAll Push Notification with first object as:");
//             _logger.LogInformation(data[0].ToString());
//             Stopwatch clock = Stopwatch.StartNew();

//             try
//             {
//                 List<Message> messagesData = new List<Message>();
//                 // PushNotificationData lastItem = null;
//                 // List<PushNotificationData> toSyncQueueList = new List<PushNotificationData>();
//                 foreach (var item in data)
//                 {
//                     // if (lastItem is null || lastItem.UserId != item.UserId)
//                     //     toSyncQueueList.Add(item);
//                     // lastItem = item;

//                     if (string.IsNullOrWhiteSpace(item.NotificationToken))
//                         continue;

//                     messagesData.Add(new Message()
//                     {
//                         Token = item.NotificationToken,
//                         Data = item.MergedData,
//                         Notification = new FirebaseAdmin.Messaging.Notification()
//                         {
//                             Title = item.TitleText,
//                             Body = item.BodyContentFormattedText,
//                             ImageUrl = item.MediaUrl,
//                         },
//                     });
//                 }
//                 if (messagesData.Count < 1)
//                     return;

//                 await StoreInSyncQueue(data);
//                 if (messagesData.Count > batchCount)
//                 {
//                     var noOfBatches = Math.Ceiling(messagesData.Count / (double)batchCount);
//                     for (var i = 0; i < noOfBatches; i++)
//                     {
//                         await FirebaseMessaging.DefaultInstance
//                             .SendAllAsync(messagesData.Skip(batchCount * i).Take(batchCount));
//                     }
//                 }
//                 else
//                     await FirebaseMessaging.DefaultInstance.SendAllAsync(messagesData);

//             }
//             catch (Exception e)
//             {
//                 _logger.LogError($"An error has occured in sending Push Notification All, count {data.Count}");
//                 _logger.LogError($"First data item as:");
//                 _logger.LogError(data[0].ToString());
//                 _logger.LogError(e.ToString());
//             }
//             finally
//             {
//                 clock.Stop();
//                 _logger.LogInformation($"Time taken for Push Notification Send All: {clock.Elapsed}");
//             }
//         }

//         public async Task SendToAllFillUserNumber(PushNotificationData data, List<LoginStatIdWithUserId> UserIds)
//         {
//             if (!(await GetSetting()) || UserIds.Count == 0)
//                 return;

//             _logger.LogInformation($"Sending push notification to token count {UserIds.Count}.");
//             // _logger.LogInformation($"Sending push notification to token count {UserIds.Count}. First object follows:");
//             // _logger.LogInformation(UserIds.FirstOrDefault()?.ToString());

//             Stopwatch clock = Stopwatch.StartNew();
//             try
//             {
//                 List<Message> messagesData = new List<Message>();
//                 List<PushNotificationData> toSyncQueueList = new List<PushNotificationData>();
//                 // PushNotificationData lastItem = null;
//                 foreach (var item in UserIds)
//                 {
//                     var targetItem = data with
//                     {
//                         ClickUserNumber = item.UserNumber.ToString(),
//                         UserId = item.UserId,
//                         LoginStatId = item.Id,
//                     };
//                     // if (lastItem is null || lastItem.UserId != item.UserId)
//                     toSyncQueueList.Add(targetItem);
//                     // lastItem = targetItem;

//                     if (string.IsNullOrWhiteSpace(item.NotificationToken))
//                     {
//                         continue;
//                         // Console.WriteLine("Empty notification token found in targetItem");
//                         // Console.WriteLine(targetItem);
//                     }
//                     messagesData.Add(new Message()
//                     {
//                         Token = item.NotificationToken,
//                         Data = targetItem.MergedData,
//                         Notification = new FirebaseAdmin.Messaging.Notification()
//                         {
//                             Title = data.TitleText,
//                             Body = targetItem.BodyContentFormattedText,
//                             ImageUrl = data.MediaUrl,
//                         },
//                     });
//                 }
//                 // if (toSyncQueueList.Count > 0)
//                 await StoreInSyncQueue(toSyncQueueList);
//                 if (messagesData.Count < 1)
//                     return;

//                 if (messagesData.Count > batchCount)
//                 {
//                     var noOfBatches = Math.Ceiling(messagesData.Count / (double)batchCount);
//                     List<Task> batchItems = new List<Task>();
//                     for (var i = 0; i < noOfBatches; i++)
//                     {
//                         batchItems.Add(FirebaseMessaging.DefaultInstance
//                                .SendAllAsync(messagesData.Skip(batchCount * i).Take(batchCount)));

//                         if (batchItems.Count == networkRequestParallelBatches || i == noOfBatches - 1)
//                         {
//                             // On every max parallel count or on last loop iteration, execute them all at once
//                             await Task.WhenAll(batchItems);
//                             batchItems.RemoveAll(_ => true); // Remove all from list
//                         }
//                     }
//                 }
//                 else
//                     await FirebaseMessaging.DefaultInstance.SendAllAsync(messagesData);
//             }
//             catch (Exception e)
//             {
//                 _logger.LogError("Could not send push notifications");
//                 _logger.LogError(e.ToString());
//             }
//             finally
//             {
//                 clock.Stop();
//                 _logger.LogInformation($"Time taken for Push Notification Send To All Fill User Number: {clock.Elapsed}");
//             }
//         }
//     }

//     public static class PushNotificationTemplates
//     {

//     }
// }