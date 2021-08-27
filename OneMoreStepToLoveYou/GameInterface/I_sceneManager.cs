using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using OneMoreStepToLoveYou.GameInterface;

namespace OneMoreStepToLoveYou.GameInterface
{
    public class I_sceneManager
    {
        public List<I_gameInterface> entites = new List<I_gameInterface>();
        List<I_gameInterface> entitesRemove = new List<I_gameInterface>();
        List<I_gameInterface> entitesAdd = new List<I_gameInterface>();

        public void Update(float animator_elapsed)
        {
            foreach (I_gameInterface item in entites)
            {
                if (entitesRemove.Contains(item))
                    continue;

                item.Update(animator_elapsed);
            }

            foreach (I_gameInterface item in entitesRemove)
            {
                entites.Remove(item);
            }

            foreach (I_gameInterface item in entitesAdd)
            {
                entites.Add(item);
            }

            entitesRemove.Clear();
            entitesAdd.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (I_gameInterface item in entites.OrderBy(e => e.DrawOrder))
            {
                item.Draw(spriteBatch);
            }
        }

        public void Remove(I_gameInterface remover)
        {
            entitesRemove.Add(remover);
        }

        public void Add(I_gameInterface adder, int drawOrder)
        {
            entitesAdd.Add(adder);
            entitesAdd[entitesAdd.Count - 1].DrawOrder = drawOrder;
        }
    }
}
