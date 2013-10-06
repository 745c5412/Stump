 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SoundAnimations")]
    [D2OClass("SoundAnimation")]
    public class SoundAnimationRecord : ID2ORecord
    {
        public uint id;
        public String name;
        public String label;
        public String filename;
        public int volume;
        public int rolloff;
        public int automationDuration;
        public int automationVolume;
        public int automationFadeIn;
        public int automationFadeOut;
        public Boolean noCutSilence;
        public uint startFrame;
        public String MODULE = "SoundAnimations";

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [NullString]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [NullString]
        public String Label
        {
            get { return label; }
            set { label = value; }
        }

        [NullString]
        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public int Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public int Rolloff
        {
            get { return rolloff; }
            set { rolloff = value; }
        }

        public int AutomationDuration
        {
            get { return automationDuration; }
            set { automationDuration = value; }
        }

        public int AutomationVolume
        {
            get { return automationVolume; }
            set { automationVolume = value; }
        }

        public int AutomationFadeIn
        {
            get { return automationFadeIn; }
            set { automationFadeIn = value; }
        }

        public int AutomationFadeOut
        {
            get { return automationFadeOut; }
            set { automationFadeOut = value; }
        }

        public Boolean NoCutSilence
        {
            get { return noCutSilence; }
            set { noCutSilence = value; }
        }

        public uint StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SoundAnimation)obj;
            
            Id = castedObj.id;
            Name = castedObj.name;
            Label = castedObj.label;
            Filename = castedObj.filename;
            Volume = castedObj.volume;
            Rolloff = castedObj.rolloff;
            AutomationDuration = castedObj.automationDuration;
            AutomationVolume = castedObj.automationVolume;
            AutomationFadeIn = castedObj.automationFadeIn;
            AutomationFadeOut = castedObj.automationFadeOut;
            NoCutSilence = castedObj.noCutSilence;
            StartFrame = castedObj.startFrame;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new SoundAnimation();
            obj.id = Id;
            obj.name = Name;
            obj.label = Label;
            obj.filename = Filename;
            obj.volume = Volume;
            obj.rolloff = Rolloff;
            obj.automationDuration = AutomationDuration;
            obj.automationVolume = AutomationVolume;
            obj.automationFadeIn = AutomationFadeIn;
            obj.automationFadeOut = AutomationFadeOut;
            obj.noCutSilence = NoCutSilence;
            obj.startFrame = StartFrame;
            return obj;
        
        }
    }
}