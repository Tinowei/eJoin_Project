using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models.Settings;
using Admin.Services;
using isRock.LineBot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi
{
    public class LineBotEventAssistantsController : LineWebHookControllerBase
    {
        private readonly LineBotSettings _lineBotSettings;
        private readonly EventConsultantService _eventConsultantService;

        public LineBotEventAssistantsController(LineBotSettings lineBotSettings,
            EventConsultantService eventConsultantService)
        {
            _lineBotSettings = lineBotSettings;
            _eventConsultantService = eventConsultantService;
        }
        [Route("api/LineBotChatGPTWebHook")]
        [HttpPost]
        public async Task<IActionResult> POST()
        {
            var adminUserId = _lineBotSettings.AdminUserId;
            try
            {
                //設定ChannelAccessToken
                this.ChannelAccessToken ="pDjqDQn95c8KhHzeNPRbVsx+xqNSlH10hxR1yfImbRPSZhvJzRrf3Uhb3+9U+Oa9WfcnM2aWpAOz8d2Qx3DqSy7Ql4LzyXdRaKnkFXNve+eVehd45cCjUaWLLwdodeDhHzZ1eGDCGJNPUyu6qZYF5gdB04t89/1O/w1cDnyilFU=";
                //配合Line Verify
                if (ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000")
                    return Ok();
                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                var responseMsg = "";
                //準備回覆訊息
                if (LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text")
                {
                    //responseMsg = ChatGPT.getResponseFromGPT(LineEvent.message.text);
                    responseMsg = await _eventConsultantService.GetSingleResponseFromAssistant(LineEvent.message.text);
                }
                else if (LineEvent.type.ToLower() == "message")
                    responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                else
                    responseMsg = $"收到 event : {LineEvent.type} ";

                //回覆訊息
                this.ReplyMessage(LineEvent.replyToken, responseMsg);
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //回覆訊息
                this.PushMessage(adminUserId, "發生錯誤:\n" + ex);
                //response OK
                return Ok();
            }
            
        }
        
    }
}
