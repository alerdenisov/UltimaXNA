﻿/***************************************************************************
 *   PartyHealthTrackerGump.cs
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using UltimaXNA.Core.Input;
using UltimaXNA.Core.UI;
using UltimaXNA.Ultima.Player;
using UltimaXNA.Ultima.UI.Controls;
using UltimaXNA.Ultima.World;
using UltimaXNA.Ultima.World.Entities.Mobiles;

namespace UltimaXNA.Ultima.UI.WorldGumps {
    class PartyHealthTrackerGump : Gump
    {
        Button btnPrivateMsg;
        GumpPic[] m_BarBGs;
        GumpPicWithWidth[] m_Bars;

        public PartyHealthTrackerGump(Serial serial)
            : base(serial, 0)
        {
            while (UserInterface.GetControl<MobileHealthTrackerGump>() != null)
            {
                UserInterface.GetControl<MobileHealthTrackerGump>(serial).Dispose();
            }
            IsMoveable = false;
            IsUncloseableWithRMB = true;
            Mobile = WorldModel.Entities.GetObject<Mobile>(serial, false);
            if (Mobile == null)
            {
                PlayerState.Partying.RemoveMember(serial);
                Dispose();
                return;
            }
            //AddControl(m_Background = new ResizePic(this, 0, 0, 3000, 131, 48));//I need opacity %1 background

            AddControl(new TextLabel(this, 1, 0, 1, Mobile.Name));
            //m_Background.MouseDoubleClickEvent += Background_MouseDoubleClickEvent; //maybe private message calling?
            m_BarBGs = new GumpPic[3];
            int sameX = 15;
            int sameY = 3;
            if (WorldModel.Entities.GetPlayerEntity().Serial != serial)//you can't send a message to self
                AddControl(btnPrivateMsg = new Button(this, 0, 20, 11401, 11402, ButtonTypes.Activate, serial, 0));//private party message / use bandage ??
            AddControl(m_BarBGs[0] = new GumpPic(this, sameX, 15 + sameY, 9750, 0));
            AddControl(m_BarBGs[1] = new GumpPic(this, sameX, 24 + sameY, 9750, 0));
            AddControl(m_BarBGs[2] = new GumpPic(this, sameX, 33 + sameY, 9750, 0));
            m_Bars = new GumpPicWithWidth[3];
            AddControl(m_Bars[0] = new GumpPicWithWidth(this, sameX, 15 + sameY, 40, 0, 1f));//I couldn't find correct visual
            AddControl(m_Bars[1] = new GumpPicWithWidth(this, sameX, 24 + sameY, 9751, 0, 1f));//I couldn't find correct visual
            AddControl(m_Bars[2] = new GumpPicWithWidth(this, sameX, 33 + sameY, 41, 0, 1f));//I couldn't find correct visual

            // bars should not handle mouse input, pass it to the background gump.
            for (int i = 0; i < m_BarBGs.Length; i++)//???
            {
                m_Bars[i].HandlesMouseInput = false;
            }
        }

        public Mobile Mobile
        {
            get;
            private set;
        }

        public override void OnButtonClick(int buttonID)
        {
            if (buttonID == 0)//private message
            {
                PlayerState.Partying.BeginPrivateMessage(btnPrivateMsg.ButtonParameter);
            }
        }

        public override void Update(double totalMS, double frameMS)
        {
            if (Mobile == null)
            {
                return;
            }
            if (PlayerState.Partying.Members.Count <= 1)
            {
                Dispose();
                return;
            }
            m_Bars[0].PercentWidthDrawn = ((float)Mobile.Health.Current / Mobile.Health.Max);

            // I couldn't find correct visual
            //if (Mobile.Flags.IsBlessed)
            //    m_Bars[0].GumpID = 0x0809;
            //else if (Mobile.Flags.IsPoisoned)
            //    m_Bars[0].GumpID = 0x0808;

            m_Bars[1].PercentWidthDrawn = ((float)Mobile.Mana.Current / Mobile.Mana.Max);

            m_Bars[2].PercentWidthDrawn = ((float)Mobile.Stamina.Current / Mobile.Stamina.Max);

            base.Update(totalMS, frameMS);
        }

        void Background_MouseDoubleClickEvent(AControl caller, int x, int y, MouseButton button)//need opacity %1 BG for this
        {
            //CALL PRIVATE MESSAGE METHOD ???
        }
    }
}