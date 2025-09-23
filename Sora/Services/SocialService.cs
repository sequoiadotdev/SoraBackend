using Microsoft.EntityFrameworkCore;
using Sora.Data;
using Sora.Models;
using Sora.Utils;

namespace Sora.Services;

public class SocialService(ApplicationDbContext db)
{
    public async Task<long> SendFriendRequest(long fromUserId, long toUserId)
    {
        if (fromUserId == toUserId)
            return -1;

        bool alreadyFriends = await db.Friends.AnyAsync(f =>
            (f.UserId == fromUserId && f.FriendUserId == toUserId) ||
            (f.UserId == toUserId && f.FriendUserId == fromUserId));

        if (alreadyFriends)
            return -1;

        bool requestExists = await db.FriendRequests.AnyAsync(r =>
            r.FromUserId == fromUserId && r.ToUserId == toUserId && r.Status == FriendRequestStatus.Pending);

        if (requestExists)
            return -1;

        var id = GlobalServices.IdGenerator.Next();
        db.FriendRequests.Add(new FriendRequest
        {
            Id = id,
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Status = FriendRequestStatus.Pending
        });

        await db.SaveChangesAsync();
        return id;
    }

    public async Task<bool> AcceptFriendRequest(long requestId)
    {
        var request = await db.FriendRequests.FindAsync(requestId);

        if (request == null || request.Status != FriendRequestStatus.Pending)
            return false;

        request.Status = FriendRequestStatus.Accepted;

        db.Friends.AddRange(
            new Friend { UserId = request.FromUserId, FriendUserId = request.ToUserId },
            new Friend { UserId = request.ToUserId, FriendUserId = request.FromUserId }
        );

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFriend(long userId, long friendId)
    {
        var friendships = await db.Friends
            .Where(f =>
                (f.UserId == userId && f.FriendUserId == friendId) ||
                (f.UserId == friendId && f.FriendUserId == userId))
            .ToListAsync();

        if (friendships.Count == 0)
            return false;

        db.Friends.RemoveRange(friendships);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<User>> GetFriendsList(long userId)
    {
        return await db.Friends
            .Where(f => f.UserId == userId)
            .Select(f => f.FriendUser)
            .ToListAsync();
    }
    
    public async Task<long> CreateGroup(string name, string description, long creatorId)
    {
        var id = GlobalServices.IdGenerator.Next();
        var group = new Group
        {
            Id = id,
            Name = name,
            Description = description,
            CreatedById = creatorId,
            CreatedAt = DateTime.UtcNow
        };

        db.Groups.Add(group);
        await db.SaveChangesAsync();

        // Add creator as admin
        db.GroupMembers.Add(new GroupMember
        {
            GroupId = group.Id,
            UserId = creatorId,
            Role = GroupRole.Admin
        });

        await db.SaveChangesAsync();
        return id;
    }

    public async Task<Group?> GetGroup(long groupId)
    {
        return await db.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
    }
    
    public async Task<bool> JoinGroup(long groupId, long userId)
    {
        bool alreadyMember = await db.GroupMembers
            .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

        if (alreadyMember)
            return false;

        db.GroupMembers.Add(new GroupMember
        {
            GroupId = groupId,
            UserId = userId,
            Role = GroupRole.Member
        });

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<User>> GetGroupMembers(long groupId)
    {
        return await db.GroupMembers
            .Where(gm => gm.GroupId == groupId)
            .Select(gm => gm.User)
            .ToListAsync();
    }

    public async Task<List<GroupMessage>> GetGroupMessages(long groupId)
    {
        return await db.GroupMessages
            .Where(m => m.GroupId == groupId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<GroupMessage?> AddGroupMessage(long groupId, long senderId, string content)
    {
        var isMember = await db.GroupMembers.AnyAsync(gm =>
            gm.GroupId == groupId && gm.UserId == senderId);

        if (!isMember)
            return null;

        var message = new GroupMessage
        {
            GroupId = groupId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow
        };

        db.GroupMessages.Add(message);
        await db.SaveChangesAsync();
        
        return message;
    }

    public async Task<bool> RemoveGroupMessage(long groupId, long messageId, long userId)
    {
        var message = await db.GroupMessages
            .Where(m => m.GroupId == groupId && m.Id == messageId)
            .FirstOrDefaultAsync();

        if (message == null || message.SenderId != userId)
            return false;

        db.GroupMessages.Remove(message);
        await db.SaveChangesAsync();
        return true;
    }
}