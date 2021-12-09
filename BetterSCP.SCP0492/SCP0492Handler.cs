// -----------------------------------------------------------------------
// <copyright file="SCP0492Handler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Mistaken.API.Diagnostics;
using UnityEngine;

namespace Mistaken.BetterSCP.SCP0492
{
    /// <inheritdoc/>
    public class SCP0492Handler : Module
    {
        /// <inheritdoc/>
        public override string Name => "SCP0492Handler";

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Player.Left += this.Player_Left;
            Exiled.Events.Handlers.Player.Verified += this.Player_Verified;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Player.Left -= this.Player_Left;
            Exiled.Events.Handlers.Player.Verified -= this.Player_Verified;
        }

        internal SCP0492Handler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private readonly Dictionary<string, (Vector3 pos, float health, int maxHealth)> leftZombieInfo = new Dictionary<string, (Vector3 pos, float health, int maxHealth)>();

        private void Player_Verified(Exiled.Events.EventArgs.VerifiedEventArgs ev)
        {
            if (!this.leftZombieInfo.TryGetValue(ev.Player.UserId, out var info))
                return;

            this.CallDelayed(2, () =>
            {
                ev.Player.Role = RoleType.Scp0492;
                ev.Player.Position = info.pos;
                this.CallDelayed(5, () =>
                {
                    ev.Player.MaxHealth = info.maxHealth;
                    ev.Player.Health = info.health;
                });
            });
        }

        private void Player_Left(Exiled.Events.EventArgs.LeftEventArgs ev)
        {
            if (ev.Player.Role == RoleType.Scp0492)
                this.leftZombieInfo[ev.Player.UserId] = (ev.Player.Position, ev.Player.Health, ev.Player.MaxHealth);
        }
    }
}
