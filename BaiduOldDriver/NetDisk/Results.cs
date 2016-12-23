﻿using System;
using System.Net;
using System.Runtime.Serialization;

namespace NetDisk
{
    [DataContract]
    public class QuotaResult
    {
        public bool success;
        [DataMember]
        public int errno;
        [DataMember]
        public long total;
        [DataMember]
        public long free;
        [DataMember]
        public bool expire;
        [DataMember]
        public long used;
        public Exception exception;
    }
    [DataContract]
    public class UserInfoResult
    {
        public bool success;
        [DataMember]
        public int errno;
        [DataMember]
        public Entry[] records;
        public Exception exception;
        [DataContract]
        public class Entry
        {
            [DataMember]
            public string avatar_url;
            [DataMember]
            public string uname;
            [DataMember]
            public string priority_name;
        }
    }
    public class LoginResult
    {
        public bool success;
        public Credential credential;
        public Exception exception;
    }
    public class LoginCheckResult
    {
        public bool success;
        public bool needVCode;
        public string codeString;
        public string verifyCode;
        public byte[] image;
        public Cookie baiduid;
        public string ltoken;
        public Exception exception;
    }
}