﻿/***************************************************************************
 *   ContextMenu.cs
 *   
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using UltimaXNA.Ultima.IO;
#endregion

namespace UltimaXNA.Ultima.Data
{
    public class ContextMenuData
    {
        private readonly List<ContextMenuItem> m_entries = new List<ContextMenuItem>();
        private readonly Serial m_serial;

        public ContextMenuData(Serial serial)
        {
            m_serial = serial;
        }

        public Serial Serial
        {
            get { return m_serial; }
        }

        public ContextMenuItem GetContextEntry(string caption)
        {
            return m_entries.FirstOrDefault(i => i.Caption == caption);
        }

        // Add a new context menu entry.
        internal void AddItem(int nResponseCode, int nStringID, int nFlags, int nHue)
        {
            m_entries.Add(new ContextMenuItem(nResponseCode, nStringID, nFlags, nHue));
        }
    }
}