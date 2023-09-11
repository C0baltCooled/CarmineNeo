using UnityEngine;
using System.IO;
using System;

[ExecuteInEditMode]
public class MapData : MonoBehaviour {
    [System.Serializable]
    public struct Header {
        [HideInInspector] public char[] charMagic;
        public string magic;
        public int version;
        public int headersize;
        public int worldsize;
        public int numents;
        public int numpvs;
        public int lightmaps;
        public int blendmap;
        public int numvars;
        public int numvslots;
    }

    [System.Serializable]
    public struct Variable {
        public byte type;
        public ushort length;
        [HideInInspector] public char[] charName;
        public string name;
        public int ivar;
        public float fvar;
        public ushort slength;
        [HideInInspector] public char[] charSVar;
        public string svar;
    }

    [System.Serializable]
    public struct Gameident {
        public byte length;
        [HideInInspector] public char[] cGame;
        public string game;
    }

    [System.Serializable]
    public struct Extras {
        public ushort extrainfosize;
        public ushort extralength;
    }

    [System.Serializable]
    public struct TextureMRU {
        public ushort texmruLength;
        public ushort[] texmru;
    }

    [System.Serializable]
    public struct Entities {
        public Vector3 position;
        public short attr1, attr2, attr3, attr4, attr5;
        public EntityType type;
        public byte reserved;
    }

    [System.Serializable]
    public struct Octree {
        public OctType type;
        public int[] textures;
        public byte mask;
    }

    public enum EntityType {
        ET_EMPTY = 0,
        ET_LIGHT = 1,
        ET_MAPMODEL = 2,
        ET_PLAYERSTART = 3,
        ET_ENVMAP = 4,
        ET_PARTICLE = 5,
        ET_SOUND = 6,
        ET_SPOTLIGHT = 7,
        ET_GAMESPECIFIC = 8
    };

    public enum OctType {
        OCTSAV_CHILDREN = 0,
        OCTSAV_EMPTY = 1,
        OCTSAV_SOLID = 2,
        OCTSAV_NORMAL = 3,
        OCTSAV_LODCUBE = 4
    }

    public enum Material {
        MAT_AIR = 0,
        MAT_WATER = 1 << 2,
        MAT_LAVA = 2 << 2,
        MAT_GLASS = 3 << 2,

        MAT_NOCLIP = 1 << 5,
        MAT_CLIP = 2 << 5,
        MAT_GAMECLIP = 3 << 5,

        MAT_DEATH = 1 << 8,
        MAT_ALPHA = 4 << 8,
        MAT_JUMP = 8 << 8
    }

    public Header header;
    public Variable[] vars;
    public Gameident game;
    public Extras extras; // Unused
    public TextureMRU texture;
    public Entities[] entities;
    public Octree[] oct;

    private void Start() {
        header.charMagic = new char[4];
        Debug.Log("Load");
        Load();
    }

    public void Save() {
        string path = Path.Combine(Application.persistentDataPath, "test");
        Stream fileStream = File.Open(path, FileMode.Create);
        fileStream.Close();
    }

    public void Load() {
        string path = Path.Combine(Application.persistentDataPath, "test");
        BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));

        // Magic
        header.charMagic = reader.ReadChars(4);
        header.magic = new string(header.charMagic); // Convert to String

        // Verison
        header.version = reader.ReadInt32();

        // Header Size
        header.headersize = reader.ReadInt32();

        // World Size
        header.worldsize = reader.ReadInt32();

        // Number of Entities
        header.numents = reader.ReadInt32();
        entities = new Entities[header.numents]; // Init Array

        // Number of PVs
        header.numpvs = reader.ReadInt32();

        // Lightmaps
        header.lightmaps = reader.ReadInt32();

        // Blendmap
        header.blendmap = reader.ReadInt32();

        // Number of Vars
        header.numvars = reader.ReadInt32();
        vars = new Variable[header.numvars]; // Init Var Array

        // Number of VSlots
        header.numvslots = reader.ReadInt32();

        // Vars
        for(int i = 0; i < header.numvars; i++) {
            vars[i].type = reader.ReadByte();
            vars[i].length = reader.ReadUInt16();

            vars[i].charName = new char[vars[i].length]; // Init Char Array
            vars[i].charName = reader.ReadChars(vars[i].length); // Read Chars
            vars[i].name = new string(vars[i].charName); // Convert to String

            // VAR (int)
            if(vars[i].type == 0) {
                vars[i].ivar = reader.ReadInt32();
            }

            // FVAR (float)
            if(vars[i].type == 1) {
                vars[i].fvar = reader.ReadSingle();
            }

            // SVAR (string)
            if(vars[i].type == 2) {
                vars[i].slength = reader.ReadUInt16();
                vars[i].charSVar = new char[vars[i].slength]; // Init Char Array
                vars[i].charSVar = reader.ReadChars(vars[i].slength); // Read Chars
                vars[i].svar = new string(vars[i].charSVar); // Convert to String
            }
        }

        // Gameident
        game.length = reader.ReadByte();
        game.cGame = reader.ReadChars(game.length);
        game.game = new string(game.cGame);

        // 0 Terminating Byte
        reader.ReadByte();

        // Extras
        extras.extrainfosize = reader.ReadUInt16();
        extras.extralength = reader.ReadUInt16();

        // Texture MRU
        texture.texmruLength = reader.ReadUInt16();
        texture.texmru = new ushort[texture.texmruLength]; // Init Array
        for(int i = 0; i < texture.texmruLength; i++) {
            texture.texmru[i] = reader.ReadUInt16();
        }

        // Entities
        for(int i = 0; i < header.numents; i++) {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();

            entities[i].position = new Vector3(x, y, z);
            entities[i].attr1 = reader.ReadInt16();
            entities[i].attr2 = reader.ReadInt16();
            entities[i].attr3 = reader.ReadInt16();
            entities[i].attr4 = reader.ReadInt16();
            entities[i].attr5 = reader.ReadInt16();
            entities[i].type = (EntityType)reader.ReadByte();
            entities[i].reserved = reader.ReadByte();
        }

        // Octree
        int worldscale = 0;
        while(1 << worldscale < header.worldsize) worldscale++;
        header.worldsize = 1 << worldscale;

        //oct = new Octree[header.worldsize >> 1];  // Init Octree Array
        Cube worldroot = loadChildren(reader, Vector3.zero, header.worldsize >> 1);
    }

    public Cube loadChildren(BinaryReader reader, Vector3 position, int size) {
        Cube c = new Cube();
        for(int i = 0; i < 8; i++) {
            //loadc(reader, c[i], new Vector3(i, position, size), size);
        }
        return c;
    }

    public void loadc() {

    }
}

public class Cube {
    public Cube children;
    ushort[] texture;
    ushort material;
}