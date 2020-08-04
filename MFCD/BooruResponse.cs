﻿using System;
using System.Collections.Generic;

namespace MFCD
{

    public partial class BooruResponse
    {
        public List<Post> Posts { get; set; }
    }

    public partial class Post
    {
        public long? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Image File { get; set; }
        public Preview Preview { get; set; }
        public Preview Sample { get; set; }
        public Score Score { get; set; }
        public Tags Tags { get; set; }
        public List<object> LockedTags { get; set; }
        public long? ChangeSeq { get; set; }
        public Flags Flags { get; set; }
        public Rating? Rating { get; set; }
        public long? FavCount { get; set; }
        public List<Uri> Sources { get; set; }
        public List<long> Pools { get; set; }
        public Relationships Relationships { get; set; }
        public long? ApproverId { get; set; }
        public long? UploaderId { get; set; }
        public string Description { get; set; }
        public long? CommentCount { get; set; }
        public bool? IsFavorited { get; set; }
        public bool? HasNotes { get; set; }
    }

    public partial class Image
    {
        public long? Width { get; set; }
        public long? Height { get; set; }
        public Ext? Ext { get; set; }
        public long? Size { get; set; }
        public string Md5 { get; set; }
        public Uri Url { get; set; }
    }

    public partial class Flags
    {
        public bool? Pending { get; set; }
        public bool? Flagged { get; set; }
        public bool? NoteLocked { get; set; }
        public bool? StatusLocked { get; set; }
        public bool? RatingLocked { get; set; }
        public bool? Deleted { get; set; }
    }

    public partial class Preview
    {
        public long? Width { get; set; }
        public long? Height { get; set; }
        public Uri Url { get; set; }
        public bool? Has { get; set; }
    }

    public partial class Relationships
    {
        public object ParentId { get; set; }
        public bool? HasChildren { get; set; }
        public bool? HasActiveChildren { get; set; }
        public List<long> Children { get; set; }
    }

    public partial class Score
    {
        public long? Up { get; set; }
        public long? Down { get; set; }
        public long? Total { get; set; }
    }

    public partial class Tags
    {
        public List<string> General { get; set; }
        public List<string> Species { get; set; }
        public List<string> Character { get; set; }
        public List<string> Copyright { get; set; }
        public List<string> Artist { get; set; }
        public List<object> Invalid { get; set; }
        public List<string> Lore { get; set; }
        public List<string> Meta { get; set; }
    }

    public enum Ext { Jpg, Png, Gif, Webm, Swf };

    public enum Rating { S };
}
