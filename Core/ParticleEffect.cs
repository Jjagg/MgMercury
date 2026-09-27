using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VenusParticleEngine.Core.Modifiers;

namespace VenusParticleEngine.Core
{
    [Serializable]
    public class ParticleEffect
    {
        public string Name { get; set; }
        public Dictionary<string, Emitter> Emitters { get; set; }
        private bool active = true;

        public ParticleEffect()
        {
            Emitters = new Dictionary<string, Emitter>();
        }

        public static ParticleEffect ReadFromJson(string jsonPath, string jsonString, GraphicsDevice graphicsDevice, ContentManager content)
        {
            try
            {
                var settings = new JsonSerializerSettings();

                settings.Converters.Add(new IModifierJsonConverter());
                ParticleEffect pf = JsonConvert.DeserializeObject<ParticleEffect>(jsonString, settings);
                pf.UpdateEmmitersTexture(NormalizePath(jsonPath), graphicsDevice, content);
                return pf;
            }
            catch (Exception e)
            {
                Console.WriteLine("Venus Particle Engine : ParticleEffect => " + e.Message);
                return null;
            }
        }

        public static ParticleEffect ReadFromJsonFile(string filePath, GraphicsDevice graphicsDevice, ContentManager content)
        {
            try
            {
                var fileContents = File.ReadAllText(filePath);
                var settings = new JsonSerializerSettings();
                var jsonPath = Path.GetDirectoryName(filePath) ?? string.Empty;

                settings.Converters.Add(new IModifierJsonConverter());
                ParticleEffect pf = JsonConvert.DeserializeObject<ParticleEffect>(fileContents, settings);
                pf.UpdateEmmitersTexture(jsonPath, graphicsDevice, content);
                return pf;
            }
            catch (Exception e)
            {
                Console.WriteLine("Venus Particle Engine : ParticleEffect (ReadFromJsonFile) => " + e.Message);
                return null;
            }
        }

        public void UpdateEmmitersTexture(String jsonPath, GraphicsDevice graphicsDevice, ContentManager content)
        {
            foreach (var item in Emitters)
            {
                item.Value.Parameters.Quantity = 1;

                var texturePath = item.Value.TexturePath ?? string.Empty;
                if (texturePath.Length == 0)
                {
                    item.Value.Texture = new Texture2D(graphicsDevice, 1, 1);
                    // item.Value.Texture.SetData<Color>(new[] { Color.White });
                }
                else
                {
                    if (jsonPath.Equals(""))
                    {
                        item.Value.Texture = LoadImage(NormalizePath(texturePath), graphicsDevice, content);
                    }
                    else
                    {
                        item.Value.Texture = LoadImage(Path.Combine(NormalizePath(jsonPath), NormalizePath(texturePath)), graphicsDevice, content);
                    }
                }

                if (item.Value.ModifierExecutionStrategy.Name.Equals("Parallel"))
                {
                    item.Value.ModifierExecutionStrategy = ModifierExecutionStrategy.Parallel();
                }
                else
                {
                    item.Value.ModifierExecutionStrategy = ModifierExecutionStrategy.Serial();
                }
            }
        }

        private Texture2D LoadImage(String fileName, GraphicsDevice graphicsDevice, ContentManager content)
        {
            if (TryLoadContentTexture(fileName, content, out var texture))
            {
                return texture;
            }

            if (Path.GetExtension(fileName).Equals(".xnb", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("LoadImageXNB Cannot open file " + fileName);
                throw new FileNotFoundException("Cannot open file", fileName);
            }

            return LoadImageNormal(fileName, graphicsDevice);
        }

        private static bool TryLoadContentTexture(string fileName, ContentManager content, out Texture2D texture)
        {
            texture = null;
            if (content == null || !TryGetContentAssetName(fileName, content, out var assetName))
                return false;

            try
            {
                texture = content.Load<Texture2D>(Path.ChangeExtension(assetName, null));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryGetContentAssetName(string fileName, ContentManager content, out string assetName)
        {
            assetName = string.Empty;
            var normalizedFileName = NormalizePath(fileName);
            if (normalizedFileName.Length == 0)
                return false;

            var contentRoot = NormalizePath(content.RootDirectory).TrimEnd(Path.DirectorySeparatorChar);
            if (Path.IsPathRooted(normalizedFileName))
            {
                var relativePath = Path.GetRelativePath(
                    Path.GetFullPath(contentRoot),
                    Path.GetFullPath(normalizedFileName));

                if (relativePath.Equals("..") || relativePath.StartsWith(".." + Path.DirectorySeparatorChar))
                    return false;

                assetName = relativePath;
            }
            else
            {
                var contentPrefix = contentRoot.Length == 0
                    ? string.Empty
                    : contentRoot + Path.DirectorySeparatorChar;

                assetName = normalizedFileName.StartsWith(contentPrefix, StringComparison.OrdinalIgnoreCase)
                    ? normalizedFileName.Substring(contentPrefix.Length)
                    : normalizedFileName;
            }

            assetName = Path.ChangeExtension(assetName, null)
                .Replace(Path.DirectorySeparatorChar, '/');

            return assetName.Length > 0;
        }

        private Texture2D LoadImageNormal(string fileName, GraphicsDevice graphicsDevice)
        {
            Texture2D spriteAtlas = null;
            try
            {
                using var fileStream = File.OpenRead(fileName);
                spriteAtlas = Texture2D.FromStream(graphicsDevice, fileStream);
                spriteAtlas.Name = fileName;
            }
            catch
            {
                Console.WriteLine("LoadImageNormal Cannot open file " + fileName);
                throw new FileNotFoundException("Cannot open file", fileName);
            }
            return spriteAtlas;
        }

        private static string NormalizePath(string path)
        {
            return string.IsNullOrEmpty(path)
                ? string.Empty
                : path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }

        public int ActiveParticles => Emitters.Sum(t => t.Value.ActiveParticles);

        public void FastForward(Vector position, float seconds, float triggerPeriod)
        {
            var time = 0f;
            while (time < seconds)
            {
                Update(triggerPeriod);
                Trigger(position);
                time += triggerPeriod;
            }
        }

        public void Update(float elapsedSeconds)
        {
            if (!active) return;
            foreach (var e in Emitters)
            {
                e.Value.Update(elapsedSeconds);
            }
        }

        public void Trigger(Vector position)
        {
            foreach (var e in Emitters)
                e.Value.Trigger(position);
        }

        public void Trigger(LineSegment line)
        {
            foreach (var e in Emitters)
                e.Value.Trigger(line);
        }

        public void Play()
        {
            active = true;
        }

        public void Pause()
        {
            active = false;
        }

        public void PlayPause()
        {
            active = !active;
        }
    }
}
