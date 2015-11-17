﻿
using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class Post
    {
        public virtual int Id { get; set; }
        // for mapping post to user with NHibernate
        public virtual User User { get; set; }

        public virtual string Title { get; set; }
        // Specialized title designed for use with URL's
        public virtual string Slug { get; set; }
        public virtual string Content { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        // "Soft delete" makes it so deleted content can still be viewed. This would be great for my wiki so users can easily review recent edits.
        public virtual DateTime? DeletedAt { get; set; }

        public virtual IList<Tag> Tags { get; set; }


        public virtual bool IsDeleted => DeletedAt != null;
    }

    public class PostMap : ClassMapping<Post>
    {
        public PostMap()
        {
            Table("posts");

            Id(x => x.Id, x => x.Generator(Generators.Identity));

            ManyToOne(x => x.User, x =>
            {
                x.Column("user_id");
                x.NotNullable(true);
            });

            Property(x => x.Title, x => x.NotNullable(true));
            Property(x => x.Slug, x => x.NotNullable(true));
            Property(x => x.Content, x => x.NotNullable(true));

            Property(x => x.CreatedAt, x =>
            {
                x.Column("created_at");
                x.NotNullable(true);
            });

            Property(x => x.UpdatedAt, x => x.Column("updated_at"));
            Property(x => x.DeletedAt, x => x.Column("updated_at"));

            Bag(x => x.Tags, x =>
            {
                x.Key(y => y.Column("post_id"));
                x.Table("post_tags");
            }, x => x.ManyToMany(y => y.Column("tag_id")));
        }
    }
}