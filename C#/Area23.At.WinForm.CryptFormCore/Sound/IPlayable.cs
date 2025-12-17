using Area23.At.Framework.Core.Util;
using Area23.At.WinForm.CryptFormCore.Properties;
using System.ComponentModel;

namespace Area23.At.WinForm.CryptFormCore.Sound
{
    internal interface IPlayable
    {
        static readonly Lock atomicLock = new Lock();

        #region Media Methods

        /// <summary>
        /// PlaySoundFromResource - plays a sound embedded in application ressource file
        /// </summary>
        /// <param name="soundName">unique qualified name for sound</param>
        internal static bool PlaySoundFromResource(string soundName)
        {
            bool played = false;
            lock (atomicLock)
            {
                UnmanagedMemoryStream stream = (UnmanagedMemoryStream)Resources.ResourceManager.GetStream(soundName);

                if (stream != null)
                {
                    try
                    {
                        // Construct the sound player
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(stream);
                        player.Play();
                        stream.Close();
                        played = true;
                    }
                    catch (Exception exSound)
                    {
                        Area23Log.LogOriginMsgEx("EncryptForm", $"PlaySoundFromResource(string soundName = {soundName})", exSound);
                        played = false;
                    }
                    finally
                    {
                        stream.Dispose();
                    }
                    //fixed (byte* bufferPtr = &bytes[0])
                    //{
                    //    System.IO.UnmanagedMemoryStream ums = new UnmanagedMemoryStream(bufferPtr, bytes.Length);
                    //    SoundPlayer player = new SoundPlayer(ums);                        
                    //    player.Play();
                    //}
                }
            }

            return played;
        }


        #endregion Media Methods
    }

    internal static class Playable_Extensions
    {

        internal static async Task<bool> PlaySoundFromResourcesAsync(this Control control, string soundName)
        {
            return await Task.Run(() => IPlayable.PlaySoundFromResource(soundName));
        }
    }


}
