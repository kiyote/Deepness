using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

public class SpriteSheetParser
{
    public class Entry
    {
        internal Entry(string name, string type, int value, Rect coordinates)
        {
            Name = name;
            Type = type;
            Value = value;
            Coordinates = coordinates;
        }

        public string Name { get; private set; }
        public string Type { get; private set; }
        public int Value { get; private set; }
        public Rect Coordinates { get; private set; }
    }

    public SpriteSheetParser()
    {
    }

    public IEnumerable<Entry> Parse(TextAsset file, float textureWidth, float textureHeight)
    {
        if (file == null)
        {
            throw new ArgumentNullException("file");
        }

        float leftOffset = 0.5f / textureWidth;
        float topOffset = 0.5f / textureHeight;

        List<Entry> result = new List<Entry>();

        string[] lines = file.text.Split(Environment.NewLine[0]);
        foreach (string row in lines)
        {
            string line = row.Trim();
            if (string.IsNullOrEmpty(line) == false)
            {
                Entry entry = ParseLine(line, leftOffset, topOffset, textureWidth, textureHeight);
                if (entry != null)
                {
                    result.Add(entry);
                }
            }
        }

        return result;
    }

    private Entry ParseLine(string line, float leftOffset, float topOffset, float textureWidth, float textureHeight)
    {
        // It's a comment line, just return
        if (line.StartsWith("#"))
        {
            return null;
        }

        string[] sides = line.Split('=');

        // If there weren't exactly two parts this isn't a valid line
        if (sides.Length != 2)
        {
            return null;
        }

        string[] descriptors = sides[0].Trim().Split('_');

        // If there weren't exactly three parts to the descriptor it isn't valid
        if (descriptors.Length != 3)
        {
            return null;
        }

        string[] coordinates = sides[1].Trim().Split(' ');

        // If there aren't exactly four coordinates it isn't valid
        if (coordinates.Length != 4)
        {
            return null;
        }

        string name = descriptors[0].Trim();
        string type = descriptors[1].Trim();
        int value = int.Parse(descriptors[2].Trim());

        float left = float.Parse(coordinates[0].Trim()) + leftOffset;
        float top = float.Parse(coordinates[1].Trim()) + topOffset;
        float width = float.Parse(coordinates[2].Trim());
        float height = float.Parse(coordinates[3].Trim());

        Rect rect = new Rect(left / textureWidth, (textureHeight - (top + height)) / textureHeight, (left + width) / textureWidth, (textureHeight - top) / textureHeight);
        return new Entry(name, type, value, rect);
    }
}
