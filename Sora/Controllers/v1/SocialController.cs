using Microsoft.AspNetCore.Mvc;
using Sora.DTOs;
using Sora.Services;

namespace Sora.Controllers.v1;
[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class SocialController(SocialService service) : ControllerBase
{
    [HttpPost("friends/request")]
    public async Task<IActionResult> FriendRequest([FromBody] FriendRequest friendRequest)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var userId = long.Parse(User.Claims.First().Value);

        var id = await service.SendFriendRequest(userId, friendRequest.To);
        
        return (id > 0) ? 
            Ok(new { RequestId = id }) :
            BadRequest(new { Error = "Failed to send friend request" });
    }

    [HttpPost("friends/accept")]
    public async Task<IActionResult> FriendAccept([FromBody] AcceptFriendRequest request)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var userId = long.Parse(User.Claims.First().Value);

        var success = await service.AcceptFriendRequest(request.RequestId);
        return success ? Ok() : BadRequest(new { Error = "Failed to accept friend request" });
    }

    [HttpDelete("friends/remove")]
    public async Task<IActionResult> FriendDelete([FromBody] FriendRemoveRequest request)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var userId = long.Parse(User.Claims.First().Value);

        var success = await service.RemoveFriend(userId, request.UserId);
        
        return success ? Ok() : BadRequest(new { Error = "Failed to remove friend" });
    }

    [HttpGet("friends/list/{id:long}")]
    public async Task<IActionResult> GetFriendsList(long id)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var userId = long.Parse(User.Claims.First().Value);

        var friendsList = await service.GetFriendsList(userId);
        
        return Ok(friendsList);
    }
    
    // Leaderboard
    
    [HttpGet("leaderboard/global")]
    public async Task<IActionResult> GetGlobalLeaderboards()
    {
        return Forbid();
    }

    [HttpGet("leaderboard/friends/{id:long}")]
    public async Task<IActionResult> GetFriendsLeadboard(long id)
    {
        return Forbid();
    }
    
    // groups

    [HttpPost("groups/create")]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var userId = long.Parse(User.Claims.First().Value);

        var groupId = await service.CreateGroup(request.Name, request.Description, userId);
        
        return Ok(groupId);
    }

    [HttpGet("groups/{id:long}")]
    public async Task<IActionResult> GetGroup(long id)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var group = await service.GetGroup(id);

        if (group is null)
        {
            return NotFound();
        }
        
        return Ok(new
        {
            group.Id,
            group.Name,
            group.Description,
            Creator = group.CreatedById,
            group.CreatedAt
        });
    }

    [HttpPost("groups/{id:long}/join")]
    public async Task<IActionResult> GroupJoin(long id) {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var userId = long.Parse(User.Claims.First().Value);

        var a = await service.JoinGroup(id, userId);
        
        return Ok();
    }

    [HttpPost("groups/{id:long}/invite")]
    public async Task<IActionResult> GroupInvite(long id)
    {
        return Ok();
    }

    [HttpPost("groups/{groupId:long}/invite/{inviteId:long}/accept")]
    public async Task<IActionResult> GroupInviteAccept(long groupId, long inviteId)
    {
        return Ok();
    }

    [HttpPost("groups/{groupId:long}/invite/{inviteId:long}/deny")]
    public async Task<IActionResult> GroupInviteDeny(long groupId, long inviteId)
    {
        return Ok();
    }

    [HttpDelete("groups/{groupId:long}/invite/{inviteId:long}")]
    public async Task<IActionResult> RemoveGroupInvite(long groupId, long inviteId)
    {
        return Ok();
    }
    
    [HttpPost("groups/{id:long}/leave")]
    public async Task<IActionResult> GroupLeave(long id)
    {
        return Ok();
    }

    [HttpGet("groups/{id:long}/members")]
    public async Task<IActionResult> GetGroupMembers(long id)
    {
        return Ok();
    }

    [HttpGet("groups/{id:long}/messages")]
    public async Task<IActionResult> GetGroupMessages(long id)
    {
        return Ok();
    }

    [HttpDelete("groups/{groupId:long}/messages/{messageId:long}")]
    public async Task<IActionResult> RemoveGroupMessage(long groupId, long messageId)
    {
        return Ok();
    }
    
    [HttpPost("groups/{id:long}/messages")]
    public async Task<IActionResult> AddGroupMessage(long id)
    {
        return Ok();
    }
}