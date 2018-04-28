﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundCloud.Models
{
    public class PublisherMetadata
    {
        public int id { get; set; }
        public string urn { get; set; }
        public string artist { get; set; }
        public string album_title { get; set; }
        public string publisher { get; set; }
        public string iswc { get; set; }
        public string upc_or_ean { get; set; }
        public string isrc { get; set; }
        public string p_line { get; set; }
        public string p_line_for_display { get; set; }
        public string writer_composer { get; set; }
        public string release_title { get; set; }
    }

    public class TopTrackUser
    {
        public string avatar_url { get; set; }
        public string first_name { get; set; }
        public string full_name { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public DateTime last_modified { get; set; }
        public string last_name { get; set; }
        public string permalink { get; set; }
        public string permalink_url { get; set; }
        public string uri { get; set; }
        public string urn { get; set; }
        public string username { get; set; }
        public bool verified { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
    }

    public class TopTrack
    {
        public string artwork_url { get; set; }
        public bool commentable { get; set; }
        public int comment_count { get; set; }
        public DateTime created_at { get; set; }
        public string description { get; set; }
        public bool downloadable { get; set; }
        public int download_count { get; set; }
        public string download_url { get; set; }
        public int duration { get; set; }
        public int full_duration { get; set; }
        public string embeddable_by { get; set; }
        public string genre { get; set; }
        public bool has_downloads_left { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public object label_name { get; set; }
        public DateTime last_modified { get; set; }
        public string license { get; set; }
        public int likes_count { get; set; }
        public string permalink { get; set; }
        public string permalink_url { get; set; }
        public int playback_count { get; set; }
        public bool @public { get; set; }
        public PublisherMetadata publisher_metadata { get; set; }
        public string purchase_title { get; set; }
        public string purchase_url { get; set; }
        public DateTime? release_date { get; set; }
        public int reposts_count { get; set; }
        public object secret_token { get; set; }
        public string sharing { get; set; }
        public string state { get; set; }
        public bool streamable { get; set; }
        public string tag_list { get; set; }
        public string title { get; set; }
        public string uri { get; set; }
        public string urn { get; set; }
        public int user_id { get; set; }
        public object visuals { get; set; }
        public string waveform_url { get; set; }
        public DateTime display_date { get; set; }
        public string monetization_model { get; set; }
        public string policy { get; set; }
        public TopTrackUser user { get; set; }
    }

    public class Collection
    {
        public TopTrack track { get; set; }
        public double score { get; set; }
    }

    public class Charts
    {
        public string genre { get; set; }
        public string kind { get; set; }
        public DateTime last_updated { get; set; }
        public List<Collection> collection { get; set; }
        public string query_urn { get; set; }
        public string next_href { get; set; }
    }
}
