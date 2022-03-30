using ChatVia.Client.Shared.UserPanel;
using ChatVia.Domain.Entities;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace ChatVia.Client.Shared.ChatPanel;

public partial class ChatPanelComponent
{
    private string MessageValue = "";

    [CascadingParameter]
    public MainLayout MainLayout { get; set; }

    private void OpenDialog()
    {
        if (response.Data is not null)
        {
            var dialogOptions = new DialogOptions()
            { 
                CloseOnEscapeKey = true, 
                CloseButton = true, 
                NoHeader = true, 
                Position = DialogPosition.TopCenter, 
                MaxWidth = MaxWidth.ExtraLarge, 
            };

            var dialogParameters = new DialogParameters
            {
                { nameof(UserPanelComponent.Username), response.Data.Member.UserName },
                { nameof(UserPanelComponent.ImageUrl), response.Data.Member.ProfilePhoto },
                { nameof(UserPanelComponent.Bio), response.Data.Member.Bio },
                { nameof(UserPanelComponent.IsOnline), true }
            };

            DialogService.Show<UserPanelComponent>("", dialogParameters, dialogOptions);
        }
    }

    private ResponseModel<ChatDto> response = new();

    protected override async Task OnParametersSetAsync()
    {
        if(MainLayout?.CurrentUserId is not null)
        {
            await _fetch.GetAsync<ResponseModel<ChatDto>>(
                $"api/chats?chatId={ MainLayout.CurrentUserId }",
                includeCredentials: true,
                callback: r => { 
                    response = r; 
                    Join(response.Data?.Id);
                    StateHasChanged();
                });
        }

        await base.OnParametersSetAsync();
    }

    private List<MessageDto> SortByDate(List<MessageDto> messages)
    {
        var ms = from m in messages
                       orderby m.CreationTime descending
                       select m;
        return ms.ToList();
    } 

    private async Task OnMuteButtonClick()
    {
        if(response.Data is not null)
        {
            string url = (response.Data.IsMuted ? "api/chats/unmute-chat" : "api/chats/mute-chat");

            var headers = new Dictionary<string, string>();
            headers.Add("chatId", response.Data.Id);

            await _fetch.PostAsync<ResponseModel<string>>(
                url,
                headers,
                includeCredentials: true,
                callback: _ => response.Data.IsMuted = !response.Data.IsMuted);

            return;
        }

        throw new ArgumentNullException(nameof(response.Data));
    }

    // SignalR 
    private HubConnection? hubConnection;

    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
            .Build();

        hubConnection.On<AppUserByIdDto, MessageDto>("ReceiveMessage", (user, message) =>
        {
            response.Data?.Messages.Add(message);
            StateHasChanged();
        });

        await hubConnection.StartAsync();
    }

    private async Task Send()
    {
        if (hubConnection is not null && response.Data is not null)
        {

            var username = (await localStorage.GetItemAsync<string>("username") ?? 
                await sessionStorage.GetItemAsync<string>("username"));

            if(username is not null)
            {
                await hubConnection.InvokeAsync("SendMessage", 
                    username,
                    response.Data.Id,
                    MessageValue);

                MessageValue = "";
            }
        }
    }

    private async Task Join(string? chatId)
    {
        if (hubConnection is not null && chatId is not null)
        {
            await hubConnection.InvokeAsync("JoinGroup", chatId);
        }
    }

    public bool IsConnected =>
        hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}
