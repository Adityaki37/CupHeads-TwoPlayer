Imports System

' Token: 0x020002FB RID: 763
Public Module WeaponProperties
	' Token: 0x06000863 RID: 2147 RVA: 0x00079A28 File Offset: 0x00077E28
	Public Function GetDisplayName(weapon As Weapon) As String
		Dim translationElement As TranslationElement = Localization.Find(weapon.ToString() + "_name")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x06000864 RID: 2148 RVA: 0x00079A6C File Offset: 0x00077E6C
	Public Function GetDisplayName(super As Super) As String
		Dim translationElement As TranslationElement = Localization.Find(super.ToString() + "_name")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x06000865 RID: 2149 RVA: 0x00079AB0 File Offset: 0x00077EB0
	Public Function GetDisplayName(charm As Charm) As String
		Dim translationElement As TranslationElement = Localization.Find(charm.ToString() + "_name")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x06000866 RID: 2150 RVA: 0x00079AF4 File Offset: 0x00077EF4
	Public Function GetSubtext(weapon As Weapon) As String
		Dim translationElement As TranslationElement = Localization.Find(weapon.ToString() + "_subtext")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x06000867 RID: 2151 RVA: 0x00079B38 File Offset: 0x00077F38
	Public Function GetSubtext(super As Super) As String
		Dim translationElement As TranslationElement = Localization.Find(super.ToString() + "_subtext")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x06000868 RID: 2152 RVA: 0x00079B7C File Offset: 0x00077F7C
	Public Function GetSubtext(charm As Charm) As String
		Dim translationElement As TranslationElement = Localization.Find(charm.ToString() + "_subtext")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x06000869 RID: 2153 RVA: 0x00079BC0 File Offset: 0x00077FC0
	Public Function GetIconPath(weapon As Weapon) As String
		If weapon = Weapon.level_weapon_peashot Then
			Return "Icons/equip_icon_weapon_peashot"
		End If
		If weapon = Weapon.level_weapon_spreadshot Then
			Return "Icons/equip_icon_weapon_spread"
		End If
		If weapon = Weapon.plane_weapon_peashot Then
			Return "Icons/equip_icon_weapon_peashot"
		End If
		If weapon = Weapon.level_weapon_arc Then
			Return "Icons/equip_icon_weapon_peashot"
		End If
		If weapon = Weapon.level_weapon_homing Then
			Return "Icons/equip_icon_weapon_homing"
		End If
		If weapon = Weapon.level_weapon_exploder Then
			Return "Icons/"
		End If
		If weapon = Weapon.level_weapon_charge Then
			Return "Icons/equip_icon_weapon_charge"
		End If
		If weapon = Weapon.level_weapon_boomerang Then
			Return "Icons/equip_icon_weapon_boomerang"
		End If
		If weapon = Weapon.level_weapon_bouncer Then
			Return "Icons/equip_icon_weapon_bouncer"
		End If
		If weapon = Weapon.arcade_weapon_peashot Then
			Return "Icons/"
		End If
		If weapon = Weapon.plane_weapon_laser Then
			Return "Icons/"
		End If
		If weapon = Weapon.level_weapon_wide_shot Then
			Return "Icons/equip_icon_weapon_wide_shot"
		End If
		If weapon = Weapon.plane_weapon_bomb Then
			Return "Icons/"
		End If
		If weapon = Weapon.arcade_weapon_rocket_peashot Then
			Return "Icons/"
		End If
		If weapon = Weapon.plane_chalice_weapon_3way Then
			Return "Icons/equip_icon_chalice_shmup_3way"
		End If
		If weapon = Weapon.level_weapon_accuracy Then
			Return "Icons/"
		End If
		If weapon = Weapon.level_weapon_firecracker Then
			Return "Icons/"
		End If
		If weapon = Weapon.level_weapon_firecrackerB Then
			Return "Icons/"
		End If
		If weapon = Weapon.level_weapon_upshot Then
			Return "Icons/equip_icon_weapon_upshot"
		End If
		If weapon = Weapon.level_weapon_pushback Then
			Return "Icons/"
		End If
		If weapon = Weapon.plane_chalice_weapon_bomb Then
			Return "Icons/equip_icon_chalice_shmup_bomb"
		End If
		If weapon = Weapon.level_weapon_crackshot Then
			Return "Icons/equip_icon_weapon_crackshot"
		End If
		If weapon = Weapon.level_weapon_splitter Then
			Return "Icons/"
		End If
		If weapon <> Weapon.None Then
			Return "ERROR"
		End If
		Return "Icons/equip_icon_empty"
	End Function

	' Token: 0x0600086A RID: 2154 RVA: 0x00079D70 File Offset: 0x00078170
	Public Function GetIconPath(super As Super) As String
		If super = Super.level_super_beam Then
			Return "Icons/equip_icon_super_beam"
		End If
		If super = Super.level_super_ghost Then
			Return "Icons/equip_icon_super_ghost"
		End If
		If super = Super.level_super_invincible Then
			Return "Icons/equip_icon_super_invincible"
		End If
		If super = Super.plane_super_bomb Then
			Return "Icons/"
		End If
		If super = Super.plane_super_chalice_bomb Then
			Return "Icons/"
		End If
		If super = Super.level_super_chalice_iii Then
			Return "Icons/equip_icon_super_ghost"
		End If
		If super = Super.level_super_chalice_vert_beam Then
			Return "Icons/equip_icon_super_beam"
		End If
		If super = Super.level_super_chalice_shield Then
			Return "Icons/equip_icon_super_invincible"
		End If
		If super = Super.level_super_chalice_bounce Then
			Return "Icons/"
		End If
		If super <> Super.None Then
			Return "ERROR"
		End If
		Return "Icons/equip_icon_empty"
	End Function

	' Token: 0x0600086B RID: 2155 RVA: 0x00079E34 File Offset: 0x00078234
	Public Function GetIconPath(charm As Charm) As String
		If charm = Charm.charm_health_up_1 Then
			Return "Icons/equip_icon_charm_hp1"
		End If
		If charm = Charm.charm_super_builder Then
			Return "Icons/equip_icon_charm_coffee"
		End If
		If charm = Charm.charm_smoke_dash Then
			Return "Icons/equip_icon_charm_smoke-dash"
		End If
		If charm = Charm.charm_parry_plus Then
			Return "Icons/equip_icon_charm_parry_slapper"
		End If
		If charm = Charm.charm_pit_saver Then
			Return "Icons/equip_icon_charm_pitsaver"
		End If
		If charm = Charm.charm_parry_attack Then
			Return "Icons/equip_icon_charm_parry_attack"
		End If
		If charm = Charm.charm_health_up_2 Then
			Return "Icons/equip_icon_charm_hp2"
		End If
		If charm = Charm.charm_chalice Then
			Return "Icons/equip_icon_charm_chalice"
		End If
		If charm = Charm.charm_directional_dash Then
			Return "Icons/"
		End If
		If charm = Charm.charm_healer Then
			Return "Icons/equip_icon_charm_healer"
		End If
		If charm = Charm.charm_EX Then
			Return "Icons/"
		End If
		If charm = Charm.charm_curse Then
			Return "Icons/equip_icon_charm_curse"
		End If
		If charm = Charm.charm_float Then
			Return "Icons/"
		End If
		If charm <> Charm.None Then
			Return "ERROR"
		End If
		Return "Icons/equip_icon_empty"
	End Function

	' Token: 0x0600086C RID: 2156 RVA: 0x00079F3C File Offset: 0x0007833C
	Public Function GetDescription(weapon As Weapon) As String
		Dim translationElement As TranslationElement = Localization.Find(weapon.ToString() + "_description")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x0600086D RID: 2157 RVA: 0x00079F80 File Offset: 0x00078380
	Public Function GetDescription(super As Super) As String
		Dim translationElement As TranslationElement = Localization.Find(super.ToString() + "_description")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x0600086E RID: 2158 RVA: 0x00079FC4 File Offset: 0x000783C4
	Public Function GetDescription(charm As Charm) As String
		Dim translationElement As TranslationElement = Localization.Find(charm.ToString() + "_description")
		If translationElement Is Nothing Then
			Return "ERROR"
		End If
		Return translationElement.translation.text
	End Function

	' Token: 0x0600086F RID: 2159 RVA: 0x0007A008 File Offset: 0x00078408
	Public Function GetValue(weapon As Weapon) As Integer
		If weapon = Weapon.level_weapon_peashot Then
			Return 2
		End If
		If weapon = Weapon.level_weapon_spreadshot Then
			Return 4
		End If
		If weapon = Weapon.plane_weapon_peashot Then
			Return 2
		End If
		If weapon = Weapon.level_weapon_arc Then
			Return 2
		End If
		If weapon = Weapon.level_weapon_homing Then
			Return 4
		End If
		If weapon = Weapon.level_weapon_exploder Then
			Return 2
		End If
		If weapon = Weapon.level_weapon_charge Then
			Return 4
		End If
		If weapon = Weapon.level_weapon_boomerang Then
			Return 4
		End If
		If weapon = Weapon.level_weapon_bouncer Then
			Return 4
		End If
		If weapon = Weapon.arcade_weapon_peashot Then
			Return 2
		End If
		If weapon = Weapon.plane_weapon_laser Then
			Return 2
		End If
		If weapon = Weapon.level_weapon_wide_shot Then
			Return 4
		End If
		If weapon = Weapon.plane_weapon_bomb Then
			Return 2
		End If
		If weapon = Weapon.arcade_weapon_rocket_peashot Then
			Return 10
		End If
		If weapon = Weapon.plane_chalice_weapon_3way Then
			Return 10
		End If
		If weapon = Weapon.level_weapon_accuracy Then
			Return 4
		End If
		If weapon = Weapon.level_weapon_firecracker Then
			Return 4
		End If
		If weapon = Weapon.level_weapon_firecrackerB Then
			Return 4
		End If
		If weapon = Weapon.level_weapon_upshot Then
			Return 4
		End If
		If weapon = Weapon.level_weapon_pushback Then
			Return 4
		End If
		If weapon = Weapon.plane_chalice_weapon_bomb Then
			Return 10
		End If
		If weapon = Weapon.level_weapon_crackshot Then
			Return 4
		End If
		If weapon <> Weapon.level_weapon_splitter Then
			Return 0
		End If
		Return 10
	End Function

	' Token: 0x06000870 RID: 2160 RVA: 0x0007A14C File Offset: 0x0007854C
	Public Function GetValue(super As Super) As Integer
		If super = Super.level_super_beam Then
			Return 0
		End If
		If super = Super.level_super_ghost Then
			Return 0
		End If
		If super = Super.level_super_invincible Then
			Return 0
		End If
		If super = Super.plane_super_bomb Then
			Return 10
		End If
		If super = Super.plane_super_chalice_bomb Then
			Return 10
		End If
		If super = Super.level_super_chalice_iii Then
			Return 10
		End If
		If super = Super.level_super_chalice_vert_beam Then
			Return 10
		End If
		If super = Super.level_super_chalice_shield Then
			Return 10
		End If
		If super <> Super.level_super_chalice_bounce Then
			Return 0
		End If
		Return 10
	End Function

	' Token: 0x06000871 RID: 2161 RVA: 0x0007A1DC File Offset: 0x000785DC
	Public Function GetValue(charm As Charm) As Integer
		If charm = Charm.charm_health_up_1 Then
			Return 3
		End If
		If charm = Charm.charm_super_builder Then
			Return 3
		End If
		If charm = Charm.charm_smoke_dash Then
			Return 3
		End If
		If charm = Charm.charm_parry_plus Then
			Return 3
		End If
		If charm = Charm.charm_pit_saver Then
			Return 3
		End If
		If charm = Charm.charm_parry_attack Then
			Return 3
		End If
		If charm = Charm.charm_health_up_2 Then
			Return 5
		End If
		If charm = Charm.charm_chalice Then
			Return 10
		End If
		If charm = Charm.charm_directional_dash Then
			Return 10
		End If
		If charm = Charm.charm_healer Then
			Return 3
		End If
		If charm = Charm.charm_EX Then
			Return 4
		End If
		If charm = Charm.charm_curse Then
			Return 1
		End If
		If charm <> Charm.charm_float Then
			Return 0
		End If
		Return 10
	End Function

	' Token: 0x020002FC RID: 764
	Public NotInheritable Class ArcadeWeaponPeashot
		' Token: 0x1700015F RID: 351
		' (get) Token: 0x06000872 RID: 2162 RVA: 0x0007A29B File Offset: 0x0007869B
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.arcade_weapon_peashot)
			End Get
		End Property

		' Token: 0x17000160 RID: 352
		' (get) Token: 0x06000873 RID: 2163 RVA: 0x0007A2A7 File Offset: 0x000786A7
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.arcade_weapon_peashot)
			End Get
		End Property

		' Token: 0x17000161 RID: 353
		' (get) Token: 0x06000874 RID: 2164 RVA: 0x0007A2B3 File Offset: 0x000786B3
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.arcade_weapon_peashot)
			End Get
		End Property

		' Token: 0x040011FF RID: 4607
		Public Shared value As Integer = 2

		' Token: 0x04001200 RID: 4608
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001201 RID: 4609
		Public Shared id As Weapon = Weapon.arcade_weapon_peashot

		' Token: 0x020002FD RID: 765
		Public NotInheritable Class Basic
			' Token: 0x04001202 RID: 4610
			Public Shared damage As Single = 4F

			' Token: 0x04001203 RID: 4611
			Public Shared speed As Single = 850F

			' Token: 0x04001204 RID: 4612
			Public Shared rapidFire As Boolean

			' Token: 0x04001205 RID: 4613
			Public Shared rapidFireRate As Single
		End Class

		' Token: 0x020002FE RID: 766
		Public NotInheritable Class Ex
		End Class
	End Class

	' Token: 0x020002FF RID: 767
	Public NotInheritable Class ArcadeWeaponRocketPeashot
		' Token: 0x17000162 RID: 354
		' (get) Token: 0x06000877 RID: 2167 RVA: 0x0007A2F1 File Offset: 0x000786F1
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.arcade_weapon_rocket_peashot)
			End Get
		End Property

		' Token: 0x17000163 RID: 355
		' (get) Token: 0x06000878 RID: 2168 RVA: 0x0007A2FD File Offset: 0x000786FD
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.arcade_weapon_rocket_peashot)
			End Get
		End Property

		' Token: 0x17000164 RID: 356
		' (get) Token: 0x06000879 RID: 2169 RVA: 0x0007A309 File Offset: 0x00078709
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.arcade_weapon_rocket_peashot)
			End Get
		End Property

		' Token: 0x04001206 RID: 4614
		Public Shared value As Integer = 10

		' Token: 0x04001207 RID: 4615
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001208 RID: 4616
		Public Shared id As Weapon = Weapon.arcade_weapon_rocket_peashot

		' Token: 0x02000300 RID: 768
		Public NotInheritable Class Basic
			' Token: 0x04001209 RID: 4617
			Public Shared damage As Single = 4F

			' Token: 0x0400120A RID: 4618
			Public Shared speed As Single = 700F

			' Token: 0x0400120B RID: 4619
			Public Shared rapidFire As Boolean

			' Token: 0x0400120C RID: 4620
			Public Shared rapidFireRate As Single
		End Class

		' Token: 0x02000301 RID: 769
		Public NotInheritable Class Ex
		End Class
	End Class

	' Token: 0x02000302 RID: 770
	Public NotInheritable Class CharmChalice
		' Token: 0x17000165 RID: 357
		' (get) Token: 0x0600087C RID: 2172 RVA: 0x0007A348 File Offset: 0x00078748
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_chalice)
			End Get
		End Property

		' Token: 0x17000166 RID: 358
		' (get) Token: 0x0600087D RID: 2173 RVA: 0x0007A354 File Offset: 0x00078754
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_chalice)
			End Get
		End Property

		' Token: 0x17000167 RID: 359
		' (get) Token: 0x0600087E RID: 2174 RVA: 0x0007A360 File Offset: 0x00078760
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_chalice)
			End Get
		End Property

		' Token: 0x0400120D RID: 4621
		Public Shared value As Integer = 10

		' Token: 0x0400120E RID: 4622
		Public Shared iconPath As String = "Icons/equip_icon_charm_chalice"

		' Token: 0x0400120F RID: 4623
		Public Shared id As Charm = Charm.charm_chalice
	End Class

	' Token: 0x02000303 RID: 771
	Public NotInheritable Class CharmCharmParryPlus
		' Token: 0x17000168 RID: 360
		' (get) Token: 0x06000880 RID: 2176 RVA: 0x0007A389 File Offset: 0x00078789
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_parry_plus)
			End Get
		End Property

		' Token: 0x17000169 RID: 361
		' (get) Token: 0x06000881 RID: 2177 RVA: 0x0007A395 File Offset: 0x00078795
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_parry_plus)
			End Get
		End Property

		' Token: 0x1700016A RID: 362
		' (get) Token: 0x06000882 RID: 2178 RVA: 0x0007A3A1 File Offset: 0x000787A1
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_parry_plus)
			End Get
		End Property

		' Token: 0x04001210 RID: 4624
		Public Shared value As Integer = 3

		' Token: 0x04001211 RID: 4625
		Public Shared iconPath As String = "Icons/equip_icon_charm_parry_slapper"

		' Token: 0x04001212 RID: 4626
		Public Shared id As Charm = Charm.charm_parry_plus
	End Class

	' Token: 0x02000304 RID: 772
	Public NotInheritable Class CharmCurse
		' Token: 0x1700016B RID: 363
		' (get) Token: 0x06000884 RID: 2180 RVA: 0x0007A3C9 File Offset: 0x000787C9
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_curse)
			End Get
		End Property

		' Token: 0x1700016C RID: 364
		' (get) Token: 0x06000885 RID: 2181 RVA: 0x0007A3D5 File Offset: 0x000787D5
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_curse)
			End Get
		End Property

		' Token: 0x1700016D RID: 365
		' (get) Token: 0x06000886 RID: 2182 RVA: 0x0007A3E1 File Offset: 0x000787E1
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_curse)
			End Get
		End Property

		' Token: 0x04001213 RID: 4627
		Public Shared value As Integer = 1

		' Token: 0x04001214 RID: 4628
		Public Shared iconPath As String = "Icons/equip_icon_charm_curse"

		' Token: 0x04001215 RID: 4629
		Public Shared id As Charm = Charm.charm_curse

		' Token: 0x04001216 RID: 4630
		Public Shared availableWeaponIDs As Integer() = New Integer() { 1456773641, 1456773649, 1460621839, 1466518900, 1466416941, 1467024095, 1487081743, 1568276855, 1614768724 }

		' Token: 0x04001217 RID: 4631
		Public Shared availableShmupWeaponIDs As Integer() = New Integer() { 1457006169, 1492758857 }

		' Token: 0x04001218 RID: 4632
		Public Shared healthModifierValues As Integer() = New Integer() { -2, -2, -2, -2, 0 }

		' Token: 0x04001219 RID: 4633
		Public Shared superMeterDelay As Single = 1F

		' Token: 0x0400121A RID: 4634
		Public Shared superMeterAmount As Single() = New Single() { 0F, 0.13F, 0.26F, 0.39F, 0.52F }

		' Token: 0x0400121B RID: 4635
		Public Shared smokeDashInterval As Integer() = New Integer() { 7, 4, 2, 1, 0 }

		' Token: 0x0400121C RID: 4636
		Public Shared whetstoneInterval As Integer() = New Integer() { 7, 4, 2, 1, 0 }

		' Token: 0x0400121D RID: 4637
		Public Shared healerInterval As String() = New String() { "3,3,4", "2,3,4", "1,3,4", "1,2,4", "1,2,3" }

		' Token: 0x0400121E RID: 4638
		Public Shared levelThreshold As Integer() = New Integer() { 0, 4, 8, 12, 16 }
	End Class

	' Token: 0x02000305 RID: 773
	Public NotInheritable Class CharmDirectionalDash
		' Token: 0x1700016E RID: 366
		' (get) Token: 0x06000888 RID: 2184 RVA: 0x0007A4F4 File Offset: 0x000788F4
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_directional_dash)
			End Get
		End Property

		' Token: 0x1700016F RID: 367
		' (get) Token: 0x06000889 RID: 2185 RVA: 0x0007A500 File Offset: 0x00078900
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_directional_dash)
			End Get
		End Property

		' Token: 0x17000170 RID: 368
		' (get) Token: 0x0600088A RID: 2186 RVA: 0x0007A50C File Offset: 0x0007890C
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_directional_dash)
			End Get
		End Property

		' Token: 0x0400121F RID: 4639
		Public Shared value As Integer = 10

		' Token: 0x04001220 RID: 4640
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001221 RID: 4641
		Public Shared id As Charm = Charm.charm_directional_dash
	End Class

	' Token: 0x02000306 RID: 774
	Public NotInheritable Class CharmEXCharm
		' Token: 0x17000171 RID: 369
		' (get) Token: 0x0600088C RID: 2188 RVA: 0x0007A535 File Offset: 0x00078935
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_EX)
			End Get
		End Property

		' Token: 0x17000172 RID: 370
		' (get) Token: 0x0600088D RID: 2189 RVA: 0x0007A541 File Offset: 0x00078941
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_EX)
			End Get
		End Property

		' Token: 0x17000173 RID: 371
		' (get) Token: 0x0600088E RID: 2190 RVA: 0x0007A54D File Offset: 0x0007894D
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_EX)
			End Get
		End Property

		' Token: 0x04001222 RID: 4642
		Public Shared value As Integer = 4

		' Token: 0x04001223 RID: 4643
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001224 RID: 4644
		Public Shared id As Charm = Charm.charm_EX

		' Token: 0x04001225 RID: 4645
		Public Shared planePeashotEXDebuff As Single = 0.15F
	End Class

	' Token: 0x02000307 RID: 775
	Public NotInheritable Class CharmFloat
		' Token: 0x17000174 RID: 372
		' (get) Token: 0x06000890 RID: 2192 RVA: 0x0007A57F File Offset: 0x0007897F
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_float)
			End Get
		End Property

		' Token: 0x17000175 RID: 373
		' (get) Token: 0x06000891 RID: 2193 RVA: 0x0007A58B File Offset: 0x0007898B
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_float)
			End Get
		End Property

		' Token: 0x17000176 RID: 374
		' (get) Token: 0x06000892 RID: 2194 RVA: 0x0007A597 File Offset: 0x00078997
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_float)
			End Get
		End Property

		' Token: 0x04001226 RID: 4646
		Public Shared value As Integer = 10

		' Token: 0x04001227 RID: 4647
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001228 RID: 4648
		Public Shared id As Charm = Charm.charm_float

		' Token: 0x04001229 RID: 4649
		Public Shared maxTime As Single = 2F

		' Token: 0x0400122A RID: 4650
		Public Shared falloffStartTime As Single = 1F

		' Token: 0x0400122B RID: 4651
		Public Shared minFallSpeed As Single = 0.1F

		' Token: 0x0400122C RID: 4652
		Public Shared maxFallSpeed As Single = 0.5F
	End Class

	' Token: 0x02000308 RID: 776
	Public NotInheritable Class CharmHealer
		' Token: 0x17000177 RID: 375
		' (get) Token: 0x06000894 RID: 2196 RVA: 0x0007A5F4 File Offset: 0x000789F4
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_healer)
			End Get
		End Property

		' Token: 0x17000178 RID: 376
		' (get) Token: 0x06000895 RID: 2197 RVA: 0x0007A600 File Offset: 0x00078A00
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_healer)
			End Get
		End Property

		' Token: 0x17000179 RID: 377
		' (get) Token: 0x06000896 RID: 2198 RVA: 0x0007A60C File Offset: 0x00078A0C
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_healer)
			End Get
		End Property

		' Token: 0x0400122D RID: 4653
		Public Shared value As Integer = 3

		' Token: 0x0400122E RID: 4654
		Public Shared iconPath As String = "Icons/equip_icon_charm_healer"

		' Token: 0x0400122F RID: 4655
		Public Shared id As Charm = Charm.charm_healer
	End Class

	' Token: 0x02000309 RID: 777
	Public NotInheritable Class CharmHealthUpOne
		' Token: 0x1700017A RID: 378
		' (get) Token: 0x06000898 RID: 2200 RVA: 0x0007A634 File Offset: 0x00078A34
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_health_up_1)
			End Get
		End Property

		' Token: 0x1700017B RID: 379
		' (get) Token: 0x06000899 RID: 2201 RVA: 0x0007A640 File Offset: 0x00078A40
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_health_up_1)
			End Get
		End Property

		' Token: 0x1700017C RID: 380
		' (get) Token: 0x0600089A RID: 2202 RVA: 0x0007A64C File Offset: 0x00078A4C
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_health_up_1)
			End Get
		End Property

		' Token: 0x04001230 RID: 4656
		Public Shared value As Integer = 3

		' Token: 0x04001231 RID: 4657
		Public Shared iconPath As String = "Icons/equip_icon_charm_hp1"

		' Token: 0x04001232 RID: 4658
		Public Shared id As Charm = Charm.charm_health_up_1

		' Token: 0x04001233 RID: 4659
		Public Shared healthIncrease As Integer = 1

		' Token: 0x04001234 RID: 4660
		Public Shared weaponDebuff As Single = 0.05F
	End Class

	' Token: 0x0200030A RID: 778
	Public NotInheritable Class CharmHealthUpTwo
		' Token: 0x1700017D RID: 381
		' (get) Token: 0x0600089C RID: 2204 RVA: 0x0007A684 File Offset: 0x00078A84
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_health_up_2)
			End Get
		End Property

		' Token: 0x1700017E RID: 382
		' (get) Token: 0x0600089D RID: 2205 RVA: 0x0007A690 File Offset: 0x00078A90
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_health_up_2)
			End Get
		End Property

		' Token: 0x1700017F RID: 383
		' (get) Token: 0x0600089E RID: 2206 RVA: 0x0007A69C File Offset: 0x00078A9C
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_health_up_2)
			End Get
		End Property

		' Token: 0x04001235 RID: 4661
		Public Shared value As Integer = 5

		' Token: 0x04001236 RID: 4662
		Public Shared iconPath As String = "Icons/equip_icon_charm_hp2"

		' Token: 0x04001237 RID: 4663
		Public Shared id As Charm = Charm.charm_health_up_2

		' Token: 0x04001238 RID: 4664
		Public Shared healthIncrease As Integer = 2

		' Token: 0x04001239 RID: 4665
		Public Shared weaponDebuff As Single = 0.1F
	End Class

	' Token: 0x0200030B RID: 779
	Public NotInheritable Class CharmParryAttack
		' Token: 0x17000180 RID: 384
		' (get) Token: 0x060008A0 RID: 2208 RVA: 0x0007A6D4 File Offset: 0x00078AD4
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_parry_attack)
			End Get
		End Property

		' Token: 0x17000181 RID: 385
		' (get) Token: 0x060008A1 RID: 2209 RVA: 0x0007A6E0 File Offset: 0x00078AE0
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_parry_attack)
			End Get
		End Property

		' Token: 0x17000182 RID: 386
		' (get) Token: 0x060008A2 RID: 2210 RVA: 0x0007A6EC File Offset: 0x00078AEC
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_parry_attack)
			End Get
		End Property

		' Token: 0x0400123A RID: 4666
		Public Shared value As Integer = 3

		' Token: 0x0400123B RID: 4667
		Public Shared iconPath As String = "Icons/equip_icon_charm_parry_attack"

		' Token: 0x0400123C RID: 4668
		Public Shared id As Charm = Charm.charm_parry_attack

		' Token: 0x0400123D RID: 4669
		Public Shared damage As Single = 16F

		' Token: 0x0400123E RID: 4670
		Public Shared bounce As Single
	End Class

	' Token: 0x0200030C RID: 780
	Public NotInheritable Class CharmPitSaver
		' Token: 0x17000183 RID: 387
		' (get) Token: 0x060008A4 RID: 2212 RVA: 0x0007A71E File Offset: 0x00078B1E
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_pit_saver)
			End Get
		End Property

		' Token: 0x17000184 RID: 388
		' (get) Token: 0x060008A5 RID: 2213 RVA: 0x0007A72A File Offset: 0x00078B2A
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_pit_saver)
			End Get
		End Property

		' Token: 0x17000185 RID: 389
		' (get) Token: 0x060008A6 RID: 2214 RVA: 0x0007A736 File Offset: 0x00078B36
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_pit_saver)
			End Get
		End Property

		' Token: 0x0400123F RID: 4671
		Public Shared value As Integer = 3

		' Token: 0x04001240 RID: 4672
		Public Shared iconPath As String = "Icons/equip_icon_charm_pitsaver"

		' Token: 0x04001241 RID: 4673
		Public Shared id As Charm = Charm.charm_pit_saver

		' Token: 0x04001242 RID: 4674
		Public Shared meterAmount As Single = 10F

		' Token: 0x04001243 RID: 4675
		Public Shared invulnerabilityMultiplier As Single = 1.6F
	End Class

	' Token: 0x0200030D RID: 781
	Public NotInheritable Class CharmSmokeDash
		' Token: 0x17000186 RID: 390
		' (get) Token: 0x060008A8 RID: 2216 RVA: 0x0007A772 File Offset: 0x00078B72
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_smoke_dash)
			End Get
		End Property

		' Token: 0x17000187 RID: 391
		' (get) Token: 0x060008A9 RID: 2217 RVA: 0x0007A77E File Offset: 0x00078B7E
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_smoke_dash)
			End Get
		End Property

		' Token: 0x17000188 RID: 392
		' (get) Token: 0x060008AA RID: 2218 RVA: 0x0007A78A File Offset: 0x00078B8A
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_smoke_dash)
			End Get
		End Property

		' Token: 0x04001244 RID: 4676
		Public Shared value As Integer = 3

		' Token: 0x04001245 RID: 4677
		Public Shared iconPath As String = "Icons/equip_icon_charm_smoke-dash"

		' Token: 0x04001246 RID: 4678
		Public Shared id As Charm = Charm.charm_smoke_dash
	End Class

	' Token: 0x0200030E RID: 782
	Public NotInheritable Class CharmSuperBuilder
		' Token: 0x17000189 RID: 393
		' (get) Token: 0x060008AC RID: 2220 RVA: 0x0007A7B2 File Offset: 0x00078BB2
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Charm.charm_super_builder)
			End Get
		End Property

		' Token: 0x1700018A RID: 394
		' (get) Token: 0x060008AD RID: 2221 RVA: 0x0007A7BE File Offset: 0x00078BBE
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Charm.charm_super_builder)
			End Get
		End Property

		' Token: 0x1700018B RID: 395
		' (get) Token: 0x060008AE RID: 2222 RVA: 0x0007A7CA File Offset: 0x00078BCA
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Charm.charm_super_builder)
			End Get
		End Property

		' Token: 0x04001247 RID: 4679
		Public Shared value As Integer = 3

		' Token: 0x04001248 RID: 4680
		Public Shared iconPath As String = "Icons/equip_icon_charm_coffee"

		' Token: 0x04001249 RID: 4681
		Public Shared id As Charm = Charm.charm_super_builder

		' Token: 0x0400124A RID: 4682
		Public Shared delay As Single = 1F

		' Token: 0x0400124B RID: 4683
		Public Shared amount As Single = 0.4F
	End Class

	' Token: 0x0200030F RID: 783
	Public NotInheritable Class LevelSuperBeam
		' Token: 0x1700018C RID: 396
		' (get) Token: 0x060008B0 RID: 2224 RVA: 0x0007A806 File Offset: 0x00078C06
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.level_super_beam)
			End Get
		End Property

		' Token: 0x1700018D RID: 397
		' (get) Token: 0x060008B1 RID: 2225 RVA: 0x0007A812 File Offset: 0x00078C12
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.level_super_beam)
			End Get
		End Property

		' Token: 0x1700018E RID: 398
		' (get) Token: 0x060008B2 RID: 2226 RVA: 0x0007A81E File Offset: 0x00078C1E
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.level_super_beam)
			End Get
		End Property

		' Token: 0x0400124C RID: 4684
		Public Shared value As Integer

		' Token: 0x0400124D RID: 4685
		Public Shared iconPath As String = "Icons/equip_icon_super_beam"

		' Token: 0x0400124E RID: 4686
		Public Shared id As Super = Super.level_super_beam

		' Token: 0x0400124F RID: 4687
		Public Shared time As Single = 1.25F

		' Token: 0x04001250 RID: 4688
		Public Shared damage As Single = 14.5F

		' Token: 0x04001251 RID: 4689
		Public Shared damageRate As Single = 0.25F
	End Class

	' Token: 0x02000310 RID: 784
	Public NotInheritable Class LevelSuperChaliceBounce
		' Token: 0x1700018F RID: 399
		' (get) Token: 0x060008B4 RID: 2228 RVA: 0x0007A85E File Offset: 0x00078C5E
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.level_super_chalice_bounce)
			End Get
		End Property

		' Token: 0x17000190 RID: 400
		' (get) Token: 0x060008B5 RID: 2229 RVA: 0x0007A86A File Offset: 0x00078C6A
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.level_super_chalice_bounce)
			End Get
		End Property

		' Token: 0x17000191 RID: 401
		' (get) Token: 0x060008B6 RID: 2230 RVA: 0x0007A876 File Offset: 0x00078C76
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.level_super_chalice_bounce)
			End Get
		End Property

		' Token: 0x04001252 RID: 4690
		Public Shared value As Integer = 10

		' Token: 0x04001253 RID: 4691
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001254 RID: 4692
		Public Shared id As Super = Super.level_super_chalice_bounce

		' Token: 0x04001255 RID: 4693
		Public Shared launchedVersion As Boolean = True

		' Token: 0x04001256 RID: 4694
		Public Shared damage As Single = 30F

		' Token: 0x04001257 RID: 4695
		Public Shared damageRate As Single = 1F

		' Token: 0x04001258 RID: 4696
		Public Shared maxDamage As Single = 300F

		' Token: 0x04001259 RID: 4697
		Public Shared duration As Single = 7.5F

		' Token: 0x0400125A RID: 4698
		Public Shared horizontalAcceleration As Single = 1200F

		' Token: 0x0400125B RID: 4699
		Public Shared maxHorizontalSpeed As Single = 1000F

		' Token: 0x0400125C RID: 4700
		Public Shared bounceVelocity As Single = 2250F

		' Token: 0x0400125D RID: 4701
		Public Shared bounceModifierNoJump As Single = 1F

		' Token: 0x0400125E RID: 4702
		Public Shared gravity As Single = 7000F

		' Token: 0x0400125F RID: 4703
		Public Shared enemyReboundMultiplier As Single = 2F

		' Token: 0x04001260 RID: 4704
		Public Shared enemyMultihitDelay As Single = 0.5F
	End Class

	' Token: 0x02000311 RID: 785
	Public NotInheritable Class LevelSuperChaliceIII
		' Token: 0x17000192 RID: 402
		' (get) Token: 0x060008B8 RID: 2232 RVA: 0x0007A920 File Offset: 0x00078D20
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.level_super_chalice_iii)
			End Get
		End Property

		' Token: 0x17000193 RID: 403
		' (get) Token: 0x060008B9 RID: 2233 RVA: 0x0007A92C File Offset: 0x00078D2C
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.level_super_chalice_iii)
			End Get
		End Property

		' Token: 0x17000194 RID: 404
		' (get) Token: 0x060008BA RID: 2234 RVA: 0x0007A938 File Offset: 0x00078D38
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.level_super_chalice_iii)
			End Get
		End Property

		' Token: 0x04001261 RID: 4705
		Public Shared value As Integer = 10

		' Token: 0x04001262 RID: 4706
		Public Shared iconPath As String = "Icons/equip_icon_super_ghost"

		' Token: 0x04001263 RID: 4707
		Public Shared id As Super = Super.level_super_chalice_iii

		' Token: 0x04001264 RID: 4708
		Public Shared superDuration As Single = 6.5F
	End Class

	' Token: 0x02000312 RID: 786
	Public NotInheritable Class LevelSuperChaliceShield
		' Token: 0x17000195 RID: 405
		' (get) Token: 0x060008BC RID: 2236 RVA: 0x0007A96B File Offset: 0x00078D6B
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.level_super_chalice_shield)
			End Get
		End Property

		' Token: 0x17000196 RID: 406
		' (get) Token: 0x060008BD RID: 2237 RVA: 0x0007A977 File Offset: 0x00078D77
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.level_super_chalice_shield)
			End Get
		End Property

		' Token: 0x17000197 RID: 407
		' (get) Token: 0x060008BE RID: 2238 RVA: 0x0007A983 File Offset: 0x00078D83
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.level_super_chalice_shield)
			End Get
		End Property

		' Token: 0x04001265 RID: 4709
		Public Shared value As Integer = 10

		' Token: 0x04001266 RID: 4710
		Public Shared iconPath As String = "Icons/equip_icon_super_invincible"

		' Token: 0x04001267 RID: 4711
		Public Shared id As Super = Super.level_super_chalice_shield
	End Class

	' Token: 0x02000313 RID: 787
	Public NotInheritable Class LevelSuperChaliceVertBeam
		' Token: 0x17000198 RID: 408
		' (get) Token: 0x060008C0 RID: 2240 RVA: 0x0007A9AC File Offset: 0x00078DAC
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.level_super_chalice_vert_beam)
			End Get
		End Property

		' Token: 0x17000199 RID: 409
		' (get) Token: 0x060008C1 RID: 2241 RVA: 0x0007A9B8 File Offset: 0x00078DB8
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.level_super_chalice_vert_beam)
			End Get
		End Property

		' Token: 0x1700019A RID: 410
		' (get) Token: 0x060008C2 RID: 2242 RVA: 0x0007A9C4 File Offset: 0x00078DC4
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.level_super_chalice_vert_beam)
			End Get
		End Property

		' Token: 0x04001268 RID: 4712
		Public Shared value As Integer = 10

		' Token: 0x04001269 RID: 4713
		Public Shared iconPath As String = "Icons/equip_icon_super_beam"

		' Token: 0x0400126A RID: 4714
		Public Shared id As Super = Super.level_super_chalice_vert_beam

		' Token: 0x0400126B RID: 4715
		Public Shared time As Single = 1.25F

		' Token: 0x0400126C RID: 4716
		Public Shared damage As Single = 21.5F

		' Token: 0x0400126D RID: 4717
		Public Shared damageRate As Single = 0.25F
	End Class

	' Token: 0x02000314 RID: 788
	Public NotInheritable Class LevelSuperGhost
		' Token: 0x1700019B RID: 411
		' (get) Token: 0x060008C4 RID: 2244 RVA: 0x0007AA0B File Offset: 0x00078E0B
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.level_super_ghost)
			End Get
		End Property

		' Token: 0x1700019C RID: 412
		' (get) Token: 0x060008C5 RID: 2245 RVA: 0x0007AA17 File Offset: 0x00078E17
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.level_super_ghost)
			End Get
		End Property

		' Token: 0x1700019D RID: 413
		' (get) Token: 0x060008C6 RID: 2246 RVA: 0x0007AA23 File Offset: 0x00078E23
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.level_super_ghost)
			End Get
		End Property

		' Token: 0x0400126E RID: 4718
		Public Shared value As Integer

		' Token: 0x0400126F RID: 4719
		Public Shared iconPath As String = "Icons/equip_icon_super_ghost"

		' Token: 0x04001270 RID: 4720
		Public Shared id As Super = Super.level_super_ghost

		' Token: 0x04001271 RID: 4721
		Public Shared initialSpeed As Single = 700F

		' Token: 0x04001272 RID: 4722
		Public Shared maxSpeed As Single = 1250F

		' Token: 0x04001273 RID: 4723
		Public Shared initialSpeedTime As Single = 1.8F

		' Token: 0x04001274 RID: 4724
		Public Shared maxSpeedTime As Single = 3.8F

		' Token: 0x04001275 RID: 4725
		Public Shared noHeartMaxSpeedTime As Single = 3.7F

		' Token: 0x04001276 RID: 4726
		Public Shared accelerationTime As Single = 1F

		' Token: 0x04001277 RID: 4727
		Public Shared heartSpeed As Single = 100F

		' Token: 0x04001278 RID: 4728
		Public Shared damage As Single = 5.1F

		' Token: 0x04001279 RID: 4729
		Public Shared damageRate As Single = 0.22F

		' Token: 0x0400127A RID: 4730
		Public Shared turnaroundEaseMultiplier As Single = 4F
	End Class

	' Token: 0x02000315 RID: 789
	Public NotInheritable Class LevelSuperInvincibility
		' Token: 0x1700019E RID: 414
		' (get) Token: 0x060008C8 RID: 2248 RVA: 0x0007AAB5 File Offset: 0x00078EB5
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.level_super_invincible)
			End Get
		End Property

		' Token: 0x1700019F RID: 415
		' (get) Token: 0x060008C9 RID: 2249 RVA: 0x0007AAC1 File Offset: 0x00078EC1
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.level_super_invincible)
			End Get
		End Property

		' Token: 0x170001A0 RID: 416
		' (get) Token: 0x060008CA RID: 2250 RVA: 0x0007AACD File Offset: 0x00078ECD
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.level_super_invincible)
			End Get
		End Property

		' Token: 0x0400127B RID: 4731
		Public Shared value As Integer

		' Token: 0x0400127C RID: 4732
		Public Shared iconPath As String = "Icons/equip_icon_super_invincible"

		' Token: 0x0400127D RID: 4733
		Public Shared id As Super = Super.level_super_invincible

		' Token: 0x0400127E RID: 4734
		Public Shared durationInvincible As Single = 4.85F

		' Token: 0x0400127F RID: 4735
		Public Shared durationFX As Single = 4.55F
	End Class

	' Token: 0x02000316 RID: 790
	Public NotInheritable Class LevelWeaponAccuracy
		' Token: 0x170001A1 RID: 417
		' (get) Token: 0x060008CC RID: 2252 RVA: 0x0007AB03 File Offset: 0x00078F03
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_accuracy)
			End Get
		End Property

		' Token: 0x170001A2 RID: 418
		' (get) Token: 0x060008CD RID: 2253 RVA: 0x0007AB0F File Offset: 0x00078F0F
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_accuracy)
			End Get
		End Property

		' Token: 0x170001A3 RID: 419
		' (get) Token: 0x060008CE RID: 2254 RVA: 0x0007AB1B File Offset: 0x00078F1B
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_accuracy)
			End Get
		End Property

		' Token: 0x04001280 RID: 4736
		Public Shared value As Integer = 4

		' Token: 0x04001281 RID: 4737
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001282 RID: 4738
		Public Shared id As Weapon = Weapon.level_weapon_accuracy

		' Token: 0x02000317 RID: 791
		Public NotInheritable Class Basic
			' Token: 0x04001283 RID: 4739
			Public Shared LvlOneFireRate As Single = 0.28F

			' Token: 0x04001284 RID: 4740
			Public Shared LvlOneSpeed As Single = 1200F

			' Token: 0x04001285 RID: 4741
			Public Shared LvlOneSize As Single = 1.2F

			' Token: 0x04001286 RID: 4742
			Public Shared LvlOneDamage As Single = 4F

			' Token: 0x04001287 RID: 4743
			Public Shared LvlTwoCounter As Integer = 20

			' Token: 0x04001288 RID: 4744
			Public Shared LvlTwoFireRate As Single = 0.23F

			' Token: 0x04001289 RID: 4745
			Public Shared LvlTwoSpeed As Single = 1500F

			' Token: 0x0400128A RID: 4746
			Public Shared LvlTwoSize As Single = 1.8F

			' Token: 0x0400128B RID: 4747
			Public Shared LvlTwoDamage As Single = 5.5F

			' Token: 0x0400128C RID: 4748
			Public Shared LvlThreeCounter As Integer = 40

			' Token: 0x0400128D RID: 4749
			Public Shared LvlThreeFireRate As Single = 0.2F

			' Token: 0x0400128E RID: 4750
			Public Shared LvlThreeSpeed As Single = 1900F

			' Token: 0x0400128F RID: 4751
			Public Shared LvlThreeSize As Single = 3.2F

			' Token: 0x04001290 RID: 4752
			Public Shared LvlThreeDamage As Single = 7.5F

			' Token: 0x04001291 RID: 4753
			Public Shared LvlFourCounter As Integer = 60

			' Token: 0x04001292 RID: 4754
			Public Shared LvlFourFireRate As Single = 0.18F

			' Token: 0x04001293 RID: 4755
			Public Shared LvlFourSpeed As Single = 2350F

			' Token: 0x04001294 RID: 4756
			Public Shared LvlFourSize As Single = 5F

			' Token: 0x04001295 RID: 4757
			Public Shared LvlFourDamage As Single = 8.5F
		End Class

		' Token: 0x02000318 RID: 792
		Public NotInheritable Class Ex
			' Token: 0x04001296 RID: 4758
			Public Shared exSpeed As Single = 1800F

			' Token: 0x04001297 RID: 4759
			Public Shared exDamage As Single = 25F

			' Token: 0x04001298 RID: 4760
			Public Shared exShotEquivalent As Integer = 15

			' Token: 0x04001299 RID: 4761
			Public Shared exShotSize As Single = 8F
		End Class
	End Class

	' Token: 0x02000319 RID: 793
	Public NotInheritable Class LevelWeaponArc
		' Token: 0x170001A4 RID: 420
		' (get) Token: 0x060008D2 RID: 2258 RVA: 0x0007AC2D File Offset: 0x0007902D
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_arc)
			End Get
		End Property

		' Token: 0x170001A5 RID: 421
		' (get) Token: 0x060008D3 RID: 2259 RVA: 0x0007AC39 File Offset: 0x00079039
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_arc)
			End Get
		End Property

		' Token: 0x170001A6 RID: 422
		' (get) Token: 0x060008D4 RID: 2260 RVA: 0x0007AC45 File Offset: 0x00079045
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_arc)
			End Get
		End Property

		' Token: 0x0400129A RID: 4762
		Public Shared value As Integer = 2

		' Token: 0x0400129B RID: 4763
		Public Shared iconPath As String = "Icons/equip_icon_weapon_peashot"

		' Token: 0x0400129C RID: 4764
		Public Shared id As Weapon = Weapon.level_weapon_arc

		' Token: 0x0200031A RID: 794
		Public NotInheritable Class Basic
			' Token: 0x0400129D RID: 4765
			Public Shared Movement As Integer

			' Token: 0x0400129E RID: 4766
			Public Shared launchSpeed As Single = 1600F

			' Token: 0x0400129F RID: 4767
			Public Shared gravity As Single = 2750F

			' Token: 0x040012A0 RID: 4768
			Public Shared straightShotAngle As Single = 65F

			' Token: 0x040012A1 RID: 4769
			Public Shared fireRate As Single = 0.4F

			' Token: 0x040012A2 RID: 4770
			Public Shared rapidFire As Boolean = True

			' Token: 0x040012A3 RID: 4771
			Public Shared maxNumMines As Integer = 1

			' Token: 0x040012A4 RID: 4772
			Public Shared baseDamage As Single = 14F

			' Token: 0x040012A5 RID: 4773
			Public Shared timeStateTwo As Single = 1.25F

			' Token: 0x040012A6 RID: 4774
			Public Shared damageStateTwo As Single = 7.5F

			' Token: 0x040012A7 RID: 4775
			Public Shared timeStateThree As Single = 2.5F

			' Token: 0x040012A8 RID: 4776
			Public Shared damageStateThree As Single = 11.25F

			' Token: 0x040012A9 RID: 4777
			Public Shared diagLaunchSpeed As Single = 600F

			' Token: 0x040012AA RID: 4778
			Public Shared diagGravity As Single = 1000F

			' Token: 0x040012AB RID: 4779
			Public Shared diagShotAngle As Single = 45F
		End Class

		' Token: 0x0200031B RID: 795
		Public NotInheritable Class Ex
			' Token: 0x040012AC RID: 4780
			Public Shared launchSpeed As Single = 1600F

			' Token: 0x040012AD RID: 4781
			Public Shared gravity As Single = 2750F

			' Token: 0x040012AE RID: 4782
			Public Shared damage As Single = 28F

			' Token: 0x040012AF RID: 4783
			Public Shared explodeDelay As Single = 2F
		End Class
	End Class

	' Token: 0x0200031C RID: 796
	Public NotInheritable Class LevelWeaponBoomerang
		' Token: 0x170001A7 RID: 423
		' (get) Token: 0x060008D8 RID: 2264 RVA: 0x0007AD2B File Offset: 0x0007912B
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_boomerang)
			End Get
		End Property

		' Token: 0x170001A8 RID: 424
		' (get) Token: 0x060008D9 RID: 2265 RVA: 0x0007AD37 File Offset: 0x00079137
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_boomerang)
			End Get
		End Property

		' Token: 0x170001A9 RID: 425
		' (get) Token: 0x060008DA RID: 2266 RVA: 0x0007AD43 File Offset: 0x00079143
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_boomerang)
			End Get
		End Property

		' Token: 0x040012B0 RID: 4784
		Public Shared value As Integer = 4

		' Token: 0x040012B1 RID: 4785
		Public Shared iconPath As String = "Icons/equip_icon_weapon_boomerang"

		' Token: 0x040012B2 RID: 4786
		Public Shared id As Weapon = Weapon.level_weapon_boomerang

		' Token: 0x0200031D RID: 797
		Public NotInheritable Class Basic
			' Token: 0x040012B3 RID: 4787
			Public Shared fireRate As Single = 0.25F

			' Token: 0x040012B4 RID: 4788
			Public Shared speed As Single = 1400F

			' Token: 0x040012B5 RID: 4789
			Public Shared damage As Single = 8.5F

			' Token: 0x040012B6 RID: 4790
			Public Shared xDistanceString As String = "550,450,520,480"

			' Token: 0x040012B7 RID: 4791
			Public Shared yDistanceString As String = "100,  50,  80, 70"
		End Class

		' Token: 0x0200031E RID: 798
		Public NotInheritable Class Ex
			' Token: 0x040012B8 RID: 4792
			Public Shared speed As Single = 1000F

			' Token: 0x040012B9 RID: 4793
			Public Shared damage As Single = 5F

			' Token: 0x040012BA RID: 4794
			Public Shared damageRate As Single = 0.2F

			' Token: 0x040012BB RID: 4795
			Public Shared maxDamage As Single = 35F

			' Token: 0x040012BC RID: 4796
			Public Shared xDistance As Single = 400F

			' Token: 0x040012BD RID: 4797
			Public Shared yDistance As Single = 110F

			' Token: 0x040012BE RID: 4798
			Public Shared pinkString As String = "2,3,2,4"

			' Token: 0x040012BF RID: 4799
			Public Shared hitFreezeTime As Single = 0.1F
		End Class
	End Class

	' Token: 0x0200031F RID: 799
	Public NotInheritable Class LevelWeaponBouncer
		' Token: 0x170001AA RID: 426
		' (get) Token: 0x060008DE RID: 2270 RVA: 0x0007ADFD File Offset: 0x000791FD
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_bouncer)
			End Get
		End Property

		' Token: 0x170001AB RID: 427
		' (get) Token: 0x060008DF RID: 2271 RVA: 0x0007AE09 File Offset: 0x00079209
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_bouncer)
			End Get
		End Property

		' Token: 0x170001AC RID: 428
		' (get) Token: 0x060008E0 RID: 2272 RVA: 0x0007AE15 File Offset: 0x00079215
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_bouncer)
			End Get
		End Property

		' Token: 0x040012C0 RID: 4800
		Public Shared value As Integer = 4

		' Token: 0x040012C1 RID: 4801
		Public Shared iconPath As String = "Icons/equip_icon_weapon_bouncer"

		' Token: 0x040012C2 RID: 4802
		Public Shared id As Weapon = Weapon.level_weapon_bouncer

		' Token: 0x02000320 RID: 800
		Public NotInheritable Class Basic
			' Token: 0x040012C3 RID: 4803
			Public Shared launchSpeed As Single = 1200F

			' Token: 0x040012C4 RID: 4804
			Public Shared gravity As Single = 3600F

			' Token: 0x040012C5 RID: 4805
			Public Shared bounceRatio As Single = 1.3F

			' Token: 0x040012C6 RID: 4806
			Public Shared bounceSpeedDampening As Single = 800F

			' Token: 0x040012C7 RID: 4807
			Public Shared straightExtraAngle As Single = 22.5F

			' Token: 0x040012C8 RID: 4808
			Public Shared diagonalUpExtraAngle As Single

			' Token: 0x040012C9 RID: 4809
			Public Shared diagonalDownExtraAngle As Single = 10F

			' Token: 0x040012CA RID: 4810
			Public Shared damage As Single = 11.6F

			' Token: 0x040012CB RID: 4811
			Public Shared fireRate As Single = 0.33F

			' Token: 0x040012CC RID: 4812
			Public Shared numBounces As Integer = 2
		End Class

		' Token: 0x02000321 RID: 801
		Public NotInheritable Class Ex
			' Token: 0x040012CD RID: 4813
			Public Shared launchSpeed As Single = 1600F

			' Token: 0x040012CE RID: 4814
			Public Shared gravity As Single = 2750F

			' Token: 0x040012CF RID: 4815
			Public Shared damage As Single = 28F

			' Token: 0x040012D0 RID: 4816
			Public Shared explodeDelay As Single = 2F
		End Class
	End Class

	' Token: 0x02000322 RID: 802
	Public NotInheritable Class LevelWeaponCharge
		' Token: 0x170001AD RID: 429
		' (get) Token: 0x060008E4 RID: 2276 RVA: 0x0007AECD File Offset: 0x000792CD
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_charge)
			End Get
		End Property

		' Token: 0x170001AE RID: 430
		' (get) Token: 0x060008E5 RID: 2277 RVA: 0x0007AED9 File Offset: 0x000792D9
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_charge)
			End Get
		End Property

		' Token: 0x170001AF RID: 431
		' (get) Token: 0x060008E6 RID: 2278 RVA: 0x0007AEE5 File Offset: 0x000792E5
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_charge)
			End Get
		End Property

		' Token: 0x040012D1 RID: 4817
		Public Shared value As Integer = 4

		' Token: 0x040012D2 RID: 4818
		Public Shared iconPath As String = "Icons/equip_icon_weapon_charge"

		' Token: 0x040012D3 RID: 4819
		Public Shared id As Weapon = Weapon.level_weapon_charge

		' Token: 0x02000323 RID: 803
		Public NotInheritable Class Basic
			' Token: 0x040012D4 RID: 4820
			Public Shared fireRate As Single = 0.25F

			' Token: 0x040012D5 RID: 4821
			Public Shared baseDamage As Single = 6F

			' Token: 0x040012D6 RID: 4822
			Public Shared speed As Single = 1050F

			' Token: 0x040012D7 RID: 4823
			Public Shared timeStateTwo As Single = 9999F

			' Token: 0x040012D8 RID: 4824
			Public Shared damageStateTwo As Single = 20F

			' Token: 0x040012D9 RID: 4825
			Public Shared speedStateTwo As Single = 1300F

			' Token: 0x040012DA RID: 4826
			Public Shared timeStateThree As Single = 1F

			' Token: 0x040012DB RID: 4827
			Public Shared damageStateThree As Single = 46F
		End Class

		' Token: 0x02000324 RID: 804
		Public NotInheritable Class Ex
			' Token: 0x040012DC RID: 4828
			Public Shared damage As Single = 26F

			' Token: 0x040012DD RID: 4829
			Public Shared radius As Single = 300F
		End Class
	End Class

	' Token: 0x02000325 RID: 805
	Public NotInheritable Class LevelWeaponCrackshot
		' Token: 0x170001B0 RID: 432
		' (get) Token: 0x060008EA RID: 2282 RVA: 0x0007AF83 File Offset: 0x00079383
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_crackshot)
			End Get
		End Property

		' Token: 0x170001B1 RID: 433
		' (get) Token: 0x060008EB RID: 2283 RVA: 0x0007AF8F File Offset: 0x0007938F
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_crackshot)
			End Get
		End Property

		' Token: 0x170001B2 RID: 434
		' (get) Token: 0x060008EC RID: 2284 RVA: 0x0007AF9B File Offset: 0x0007939B
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_crackshot)
			End Get
		End Property

		' Token: 0x040012DE RID: 4830
		Public Shared value As Integer = 4

		' Token: 0x040012DF RID: 4831
		Public Shared iconPath As String = "Icons/equip_icon_weapon_crackshot"

		' Token: 0x040012E0 RID: 4832
		Public Shared id As Weapon = Weapon.level_weapon_crackshot

		' Token: 0x02000326 RID: 806
		Public NotInheritable Class Basic
			' Token: 0x040012E1 RID: 4833
			Public Shared fireRate As Single = 0.32F

			' Token: 0x040012E2 RID: 4834
			Public Shared initialSpeed As Single = 1050F

			' Token: 0x040012E3 RID: 4835
			Public Shared crackDistance As Single = 290F

			' Token: 0x040012E4 RID: 4836
			Public Shared crackedSpeed As Single = 2500F

			' Token: 0x040012E5 RID: 4837
			Public Shared initialDamage As Single = 10.56F

			' Token: 0x040012E6 RID: 4838
			Public Shared crackedDamage As Single = 6.7F

			' Token: 0x040012E7 RID: 4839
			Public Shared enableMaxAngle As Boolean = True

			' Token: 0x040012E8 RID: 4840
			Public Shared maxAngle As Single = 170F
		End Class

		' Token: 0x02000327 RID: 807
		Public NotInheritable Class Ex
			' Token: 0x040012E9 RID: 4841
			Public Shared launchDistance As Single = 100F

			' Token: 0x040012EA RID: 4842
			Public Shared timeToHoverPoint As Single = 0.5F

			' Token: 0x040012EB RID: 4843
			Public Shared hoverWidth As Single = 37F

			' Token: 0x040012EC RID: 4844
			Public Shared hoverHeight As Single = 35F

			' Token: 0x040012ED RID: 4845
			Public Shared hoverSpeed As Single = 0.9F

			' Token: 0x040012EE RID: 4846
			Public Shared bulletSpeed As Single = 2000F

			' Token: 0x040012EF RID: 4847
			Public Shared bulletDamage As Single = 3.5F

			' Token: 0x040012F0 RID: 4848
			Public Shared collideDamage As Single = 12F

			' Token: 0x040012F1 RID: 4849
			Public Shared shotNumber As Integer = 5

			' Token: 0x040012F2 RID: 4850
			Public Shared shootDelay As Single = 1F

			' Token: 0x040012F3 RID: 4851
			Public Shared riseSpeed As Single

			' Token: 0x040012F4 RID: 4852
			Public Shared isPink As Boolean = True

			' Token: 0x040012F5 RID: 4853
			Public Shared parryBulletDamage As Single = 14F

			' Token: 0x040012F6 RID: 4854
			Public Shared parryBulletSpeed As Single = 2000F

			' Token: 0x040012F7 RID: 4855
			Public Shared parryTimeOut As Single = 0.15F
		End Class
	End Class

	' Token: 0x02000328 RID: 808
	Public NotInheritable Class LevelWeaponExploder
		' Token: 0x170001B3 RID: 435
		' (get) Token: 0x060008F0 RID: 2288 RVA: 0x0007B0B1 File Offset: 0x000794B1
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_exploder)
			End Get
		End Property

		' Token: 0x170001B4 RID: 436
		' (get) Token: 0x060008F1 RID: 2289 RVA: 0x0007B0BD File Offset: 0x000794BD
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_exploder)
			End Get
		End Property

		' Token: 0x170001B5 RID: 437
		' (get) Token: 0x060008F2 RID: 2290 RVA: 0x0007B0C9 File Offset: 0x000794C9
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_exploder)
			End Get
		End Property

		' Token: 0x040012F8 RID: 4856
		Public Shared value As Integer = 2

		' Token: 0x040012F9 RID: 4857
		Public Shared iconPath As String = "Icons/"

		' Token: 0x040012FA RID: 4858
		Public Shared id As Weapon = Weapon.level_weapon_exploder

		' Token: 0x02000329 RID: 809
		Public NotInheritable Class Basic
			' Token: 0x040012FB RID: 4859
			Public Shared fireRate As Single = 0.35F

			' Token: 0x040012FC RID: 4860
			Public Shared rapideFire As Boolean = True

			' Token: 0x040012FD RID: 4861
			Public Shared speed As Single = 1200F

			' Token: 0x040012FE RID: 4862
			Public Shared sinSpeed As Single = 10F

			' Token: 0x040012FF RID: 4863
			Public Shared sinSize As Single = 0.1F

			' Token: 0x04001300 RID: 4864
			Public Shared baseDamage As Single = 6F

			' Token: 0x04001301 RID: 4865
			Public Shared baseExplosionRadius As Single = 15F

			' Token: 0x04001302 RID: 4866
			Public Shared baseScale As Single = 0.1F

			' Token: 0x04001303 RID: 4867
			Public Shared timeStateTwo As Single = 0.25F

			' Token: 0x04001304 RID: 4868
			Public Shared damageStateTwo As Single = 10F

			' Token: 0x04001305 RID: 4869
			Public Shared explosionRadiusStateTwo As Single = 70F

			' Token: 0x04001306 RID: 4870
			Public Shared scaleStateTwo As Single = 0.5F

			' Token: 0x04001307 RID: 4871
			Public Shared timeStateThree As Single = 0.5F

			' Token: 0x04001308 RID: 4872
			Public Shared damageStateThree As Single = 12.75F

			' Token: 0x04001309 RID: 4873
			Public Shared explosionRadiusStateThree As Single = 130F

			' Token: 0x0400130A RID: 4874
			Public Shared scaleStateThree As Single = 1F

			' Token: 0x0400130B RID: 4875
			Public Shared easing As Boolean = True

			' Token: 0x0400130C RID: 4876
			Public Shared easeSpeed As MinMax = New MinMax(900F, 2500F)

			' Token: 0x0400130D RID: 4877
			Public Shared easeTime As Single = 1F
		End Class

		' Token: 0x0200032A RID: 810
		Public NotInheritable Class Ex
			' Token: 0x0400130E RID: 4878
			Public Shared speed As Single = 1300F

			' Token: 0x0400130F RID: 4879
			Public Shared damage As Single = 35F

			' Token: 0x04001310 RID: 4880
			Public Shared hitRate As Single

			' Token: 0x04001311 RID: 4881
			Public Shared explodeRadius As Single = 300F

			' Token: 0x04001312 RID: 4882
			Public Shared shrapnelSpeed As Single = 1200F

			' Token: 0x04001313 RID: 4883
			Public Shared damageOn As Boolean = True
		End Class
	End Class

	' Token: 0x0200032B RID: 811
	Public NotInheritable Class LevelWeaponFirecracker
		' Token: 0x170001B6 RID: 438
		' (get) Token: 0x060008F6 RID: 2294 RVA: 0x0007B1F1 File Offset: 0x000795F1
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_firecracker)
			End Get
		End Property

		' Token: 0x170001B7 RID: 439
		' (get) Token: 0x060008F7 RID: 2295 RVA: 0x0007B1FD File Offset: 0x000795FD
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_firecracker)
			End Get
		End Property

		' Token: 0x170001B8 RID: 440
		' (get) Token: 0x060008F8 RID: 2296 RVA: 0x0007B209 File Offset: 0x00079609
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_firecracker)
			End Get
		End Property

		' Token: 0x04001314 RID: 4884
		Public Shared value As Integer = 4

		' Token: 0x04001315 RID: 4885
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001316 RID: 4886
		Public Shared id As Weapon = Weapon.level_weapon_firecracker

		' Token: 0x0200032C RID: 812
		Public NotInheritable Class Basic
			' Token: 0x04001317 RID: 4887
			Public Shared fireRate As Single = 0.06F

			' Token: 0x04001318 RID: 4888
			Public Shared bulletSpeed As Single = 2250F

			' Token: 0x04001319 RID: 4889
			Public Shared bulletLife As Single = 0.17F

			' Token: 0x0400131A RID: 4890
			Public Shared explosionDamage As Single = 2.6F

			' Token: 0x0400131B RID: 4891
			Public Shared explosionSize As Single = 10F

			' Token: 0x0400131C RID: 4892
			Public Shared explosionDuration As Single = 0.1F
		End Class

		' Token: 0x0200032D RID: 813
		Public NotInheritable Class Ex
			' Token: 0x0400131D RID: 4893
			Public Shared exSpeed As Single = 1700F

			' Token: 0x0400131E RID: 4894
			Public Shared explosionRadius As Single = 20F

			' Token: 0x0400131F RID: 4895
			Public Shared damageRate As Single = 0.5F

			' Token: 0x04001320 RID: 4896
			Public Shared explosionDamage As Single = 3F

			' Token: 0x04001321 RID: 4897
			Public Shared explosionTime As Single = 2F

			' Token: 0x04001322 RID: 4898
			Public Shared exLife As Single = 1F
		End Class
	End Class

	' Token: 0x0200032E RID: 814
	Public NotInheritable Class LevelWeaponFirecrackerB
		' Token: 0x170001B9 RID: 441
		' (get) Token: 0x060008FC RID: 2300 RVA: 0x0007B2AD File Offset: 0x000796AD
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_firecrackerB)
			End Get
		End Property

		' Token: 0x170001BA RID: 442
		' (get) Token: 0x060008FD RID: 2301 RVA: 0x0007B2B9 File Offset: 0x000796B9
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_firecrackerB)
			End Get
		End Property

		' Token: 0x170001BB RID: 443
		' (get) Token: 0x060008FE RID: 2302 RVA: 0x0007B2C5 File Offset: 0x000796C5
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_firecrackerB)
			End Get
		End Property

		' Token: 0x04001323 RID: 4899
		Public Shared value As Integer = 4

		' Token: 0x04001324 RID: 4900
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001325 RID: 4901
		Public Shared id As Weapon = Weapon.level_weapon_firecrackerB

		' Token: 0x0200032F RID: 815
		Public NotInheritable Class Basic
			' Token: 0x04001326 RID: 4902
			Public Shared fireRate As Single = 0.09F

			' Token: 0x04001327 RID: 4903
			Public Shared bulletSpeed As Single = 2000F

			' Token: 0x04001328 RID: 4904
			Public Shared bulletLife As Single = 0.2F

			' Token: 0x04001329 RID: 4905
			Public Shared explosionDamage As Single = 0.5F

			' Token: 0x0400132A RID: 4906
			Public Shared explosionSize As Single = 5F

			' Token: 0x0400132B RID: 4907
			Public Shared explosionDuration As Single = 0.16F

			' Token: 0x0400132C RID: 4908
			Public Shared explosionAngleString As String = "45,180,270,135,315,225,90,0"

			' Token: 0x0400132D RID: 4909
			Public Shared explosionsRadiusSize As Single = 68F
		End Class

		' Token: 0x02000330 RID: 816
		Public NotInheritable Class Ex
			' Token: 0x0400132E RID: 4910
			Public Shared exSpeed As Single = 1700F

			' Token: 0x0400132F RID: 4911
			Public Shared explosionRadius As Single = 20F

			' Token: 0x04001330 RID: 4912
			Public Shared damageRate As Single = 0.5F

			' Token: 0x04001331 RID: 4913
			Public Shared explosionDamage As Single = 3F

			' Token: 0x04001332 RID: 4914
			Public Shared explosionTime As Single = 2F

			' Token: 0x04001333 RID: 4915
			Public Shared exLife As Single = 1F
		End Class
	End Class

	' Token: 0x02000331 RID: 817
	Public NotInheritable Class LevelWeaponHoming
		' Token: 0x170001BC RID: 444
		' (get) Token: 0x06000902 RID: 2306 RVA: 0x0007B38B File Offset: 0x0007978B
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_homing)
			End Get
		End Property

		' Token: 0x170001BD RID: 445
		' (get) Token: 0x06000903 RID: 2307 RVA: 0x0007B397 File Offset: 0x00079797
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_homing)
			End Get
		End Property

		' Token: 0x170001BE RID: 446
		' (get) Token: 0x06000904 RID: 2308 RVA: 0x0007B3A3 File Offset: 0x000797A3
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_homing)
			End Get
		End Property

		' Token: 0x04001334 RID: 4916
		Public Shared value As Integer = 4

		' Token: 0x04001335 RID: 4917
		Public Shared iconPath As String = "Icons/equip_icon_weapon_homing"

		' Token: 0x04001336 RID: 4918
		Public Shared id As Weapon = Weapon.level_weapon_homing

		' Token: 0x02000332 RID: 818
		Public NotInheritable Class Basic
			' Token: 0x04001337 RID: 4919
			Public Shared fireRate As MinMax = New MinMax(0.15F, 0.15F)

			' Token: 0x04001338 RID: 4920
			Public Shared speed As Single = 1000F

			' Token: 0x04001339 RID: 4921
			Public Shared damage As Single = 2.85F

			' Token: 0x0400133A RID: 4922
			Public Shared rotationSpeed As MinMax = New MinMax(0F, 500F)

			' Token: 0x0400133B RID: 4923
			Public Shared timeBeforeEaseRotationSpeed As Single = 0F

			' Token: 0x0400133C RID: 4924
			Public Shared rotationSpeedEaseTime As Single = 0.4F

			' Token: 0x0400133D RID: 4925
			Public Shared lockedShotAccelerationTime As Single = 0.5F

			' Token: 0x0400133E RID: 4926
			Public Shared speedVariation As Single = 100F

			' Token: 0x0400133F RID: 4927
			Public Shared angleVariation As Single = 5F

			' Token: 0x04001340 RID: 4928
			Public Shared trailFrameDelay As Integer = 2

			' Token: 0x04001341 RID: 4929
			Public Shared maxHomingTime As Single = 2.5F
		End Class

		' Token: 0x02000333 RID: 819
		Public NotInheritable Class Ex
			' Token: 0x04001342 RID: 4930
			Public Shared speed As Single = 1500F

			' Token: 0x04001343 RID: 4931
			Public Shared damage As Single = 7F

			' Token: 0x04001344 RID: 4932
			Public Shared spread As Single = 90F

			' Token: 0x04001345 RID: 4933
			Public Shared bulletCount As Integer = 4

			' Token: 0x04001346 RID: 4934
			Public Shared swirlDistance As Single = 100F

			' Token: 0x04001347 RID: 4935
			Public Shared swirlEaseTime As Single = 0.75F

			' Token: 0x04001348 RID: 4936
			Public Shared trailFrameDelay As Integer = 2
		End Class
	End Class

	' Token: 0x02000334 RID: 820
	Public NotInheritable Class LevelWeaponPeashot
		' Token: 0x170001BF RID: 447
		' (get) Token: 0x06000908 RID: 2312 RVA: 0x0007B497 File Offset: 0x00079897
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_peashot)
			End Get
		End Property

		' Token: 0x170001C0 RID: 448
		' (get) Token: 0x06000909 RID: 2313 RVA: 0x0007B4A3 File Offset: 0x000798A3
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_peashot)
			End Get
		End Property

		' Token: 0x170001C1 RID: 449
		' (get) Token: 0x0600090A RID: 2314 RVA: 0x0007B4AF File Offset: 0x000798AF
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_peashot)
			End Get
		End Property

		' Token: 0x04001349 RID: 4937
		Public Shared value As Integer = 2

		' Token: 0x0400134A RID: 4938
		Public Shared iconPath As String = "Icons/equip_icon_weapon_peashot"

		' Token: 0x0400134B RID: 4939
		Public Shared id As Weapon = Weapon.level_weapon_peashot

		' Token: 0x02000335 RID: 821
		Public NotInheritable Class Basic
			' Token: 0x0400134C RID: 4940
			Public Shared damage As Single = 4F

			' Token: 0x0400134D RID: 4941
			Public Shared speed As Single = 2250F

			' Token: 0x0400134E RID: 4942
			Public Shared rapidFire As Boolean = True

			' Token: 0x0400134F RID: 4943
			Public Shared rapidFireRate As Single = 0.11F
		End Class

		' Token: 0x02000336 RID: 822
		Public NotInheritable Class Ex
			' Token: 0x04001350 RID: 4944
			Public Shared damage As Single = 8.334F

			' Token: 0x04001351 RID: 4945
			Public Shared maxDamage As Single = 25F

			' Token: 0x04001352 RID: 4946
			Public Shared damageDistance As Single = 80F

			' Token: 0x04001353 RID: 4947
			Public Shared speed As Single = 1500F

			' Token: 0x04001354 RID: 4948
			Public Shared freezeTime As Single = 0.05F
		End Class
	End Class

	' Token: 0x02000337 RID: 823
	Public NotInheritable Class LevelWeaponPushback
		' Token: 0x170001C2 RID: 450
		' (get) Token: 0x0600090E RID: 2318 RVA: 0x0007B531 File Offset: 0x00079931
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_pushback)
			End Get
		End Property

		' Token: 0x170001C3 RID: 451
		' (get) Token: 0x0600090F RID: 2319 RVA: 0x0007B53D File Offset: 0x0007993D
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_pushback)
			End Get
		End Property

		' Token: 0x170001C4 RID: 452
		' (get) Token: 0x06000910 RID: 2320 RVA: 0x0007B549 File Offset: 0x00079949
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_pushback)
			End Get
		End Property

		' Token: 0x04001355 RID: 4949
		Public Shared value As Integer = 4

		' Token: 0x04001356 RID: 4950
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001357 RID: 4951
		Public Shared id As Weapon = Weapon.level_weapon_pushback

		' Token: 0x02000338 RID: 824
		Public NotInheritable Class Basic
			' Token: 0x04001358 RID: 4952
			Public Shared damage As Single = 4F

			' Token: 0x04001359 RID: 4953
			Public Shared fireRate As MinMax = New MinMax(0.1F, 0.7F)

			' Token: 0x0400135A RID: 4954
			Public Shared speed As MinMax = New MinMax(700F, 1300F)

			' Token: 0x0400135B RID: 4955
			Public Shared speedTime As Single = 3F

			' Token: 0x0400135C RID: 4956
			Public Shared pushbackSpeed As Single = 30F
		End Class

		' Token: 0x02000339 RID: 825
		Public NotInheritable Class Ex
		End Class
	End Class

	' Token: 0x0200033A RID: 826
	Public NotInheritable Class LevelWeaponSplitter
		' Token: 0x170001C5 RID: 453
		' (get) Token: 0x06000913 RID: 2323 RVA: 0x0007B5C7 File Offset: 0x000799C7
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_splitter)
			End Get
		End Property

		' Token: 0x170001C6 RID: 454
		' (get) Token: 0x06000914 RID: 2324 RVA: 0x0007B5D3 File Offset: 0x000799D3
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_splitter)
			End Get
		End Property

		' Token: 0x170001C7 RID: 455
		' (get) Token: 0x06000915 RID: 2325 RVA: 0x0007B5DF File Offset: 0x000799DF
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_splitter)
			End Get
		End Property

		' Token: 0x0400135D RID: 4957
		Public Shared value As Integer = 10

		' Token: 0x0400135E RID: 4958
		Public Shared iconPath As String = "Icons/"

		' Token: 0x0400135F RID: 4959
		Public Shared id As Weapon = Weapon.level_weapon_splitter

		' Token: 0x0200033B RID: 827
		Public NotInheritable Class Basic
			' Token: 0x04001360 RID: 4960
			Public Shared fireRate As Single = 0.22F

			' Token: 0x04001361 RID: 4961
			Public Shared speed As Single = 1700F

			' Token: 0x04001362 RID: 4962
			Public Shared splitDistanceA As Single = 200F

			' Token: 0x04001363 RID: 4963
			Public Shared splitDistanceB As Single = 550F

			' Token: 0x04001364 RID: 4964
			Public Shared bulletDamage As Single = 4F

			' Token: 0x04001365 RID: 4965
			Public Shared bulletDamageA As Single = 2.15F

			' Token: 0x04001366 RID: 4966
			Public Shared bulletDamageB As Single = 1.65F

			' Token: 0x04001367 RID: 4967
			Public Shared splitAngle As Single = 20F

			' Token: 0x04001368 RID: 4968
			Public Shared angleDistance As Single = 100F
		End Class

		' Token: 0x0200033C RID: 828
		Public NotInheritable Class Ex
		End Class
	End Class

	' Token: 0x0200033D RID: 829
	Public NotInheritable Class LevelWeaponSpreadshot
		' Token: 0x170001C8 RID: 456
		' (get) Token: 0x06000918 RID: 2328 RVA: 0x0007B66F File Offset: 0x00079A6F
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_spreadshot)
			End Get
		End Property

		' Token: 0x170001C9 RID: 457
		' (get) Token: 0x06000919 RID: 2329 RVA: 0x0007B67B File Offset: 0x00079A7B
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_spreadshot)
			End Get
		End Property

		' Token: 0x170001CA RID: 458
		' (get) Token: 0x0600091A RID: 2330 RVA: 0x0007B687 File Offset: 0x00079A87
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_spreadshot)
			End Get
		End Property

		' Token: 0x04001369 RID: 4969
		Public Shared value As Integer = 4

		' Token: 0x0400136A RID: 4970
		Public Shared iconPath As String = "Icons/equip_icon_weapon_spread"

		' Token: 0x0400136B RID: 4971
		Public Shared id As Weapon = Weapon.level_weapon_spreadshot

		' Token: 0x0200033E RID: 830
		Public NotInheritable Class Basic
			' Token: 0x0400136C RID: 4972
			Public Shared damage As Single = 1.24F

			' Token: 0x0400136D RID: 4973
			Public Shared speed As Single = 2250F

			' Token: 0x0400136E RID: 4974
			Public Shared distance As Single = 375F

			' Token: 0x0400136F RID: 4975
			Public Shared rapidFireRate As Single = 0.13F
		End Class

		' Token: 0x0200033F RID: 831
		Public NotInheritable Class Ex
			' Token: 0x04001370 RID: 4976
			Public Shared damage As Single = 4.3F

			' Token: 0x04001371 RID: 4977
			Public Shared speed As Single = 500F

			' Token: 0x04001372 RID: 4978
			Public Shared childCount As Integer = 8

			' Token: 0x04001373 RID: 4979
			Public Shared radius As Single = 100F
		End Class
	End Class

	' Token: 0x02000340 RID: 832
	Public NotInheritable Class LevelWeaponUpshot
		' Token: 0x170001CB RID: 459
		' (get) Token: 0x0600091E RID: 2334 RVA: 0x0007B6FF File Offset: 0x00079AFF
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_upshot)
			End Get
		End Property

		' Token: 0x170001CC RID: 460
		' (get) Token: 0x0600091F RID: 2335 RVA: 0x0007B70B File Offset: 0x00079B0B
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_upshot)
			End Get
		End Property

		' Token: 0x170001CD RID: 461
		' (get) Token: 0x06000920 RID: 2336 RVA: 0x0007B717 File Offset: 0x00079B17
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_upshot)
			End Get
		End Property

		' Token: 0x04001374 RID: 4980
		Public Shared value As Integer = 4

		' Token: 0x04001375 RID: 4981
		Public Shared iconPath As String = "Icons/equip_icon_weapon_upshot"

		' Token: 0x04001376 RID: 4982
		Public Shared id As Weapon = Weapon.level_weapon_upshot

		' Token: 0x02000341 RID: 833
		Public NotInheritable Class Basic
			' Token: 0x04001377 RID: 4983
			Public Shared damage As Single = 2.33F

			' Token: 0x04001378 RID: 4984
			Public Shared fireRate As Single = 0.2F

			' Token: 0x04001379 RID: 4985
			Public Shared xSpeed As Single() = New Single() { 630F, 819F, 945F }

			' Token: 0x0400137A RID: 4986
			Public Shared ySpeed As MinMax() = New MinMax() { New MinMax(0F, 3240F), New MinMax(0F, 3240F), New MinMax(0F, 3240F) }

			' Token: 0x0400137B RID: 4987
			Public Shared timeToMaxSpeed As Single() = New Single() { 1.08F, 0.81F, 0.945F }
		End Class

		' Token: 0x02000342 RID: 834
		Public NotInheritable Class Ex
			' Token: 0x0400137C RID: 4988
			Public Shared minRotationSpeed As Single = 375F

			' Token: 0x0400137D RID: 4989
			Public Shared maxRotationSpeed As Single = 185F

			' Token: 0x0400137E RID: 4990
			Public Shared rotationRampTime As Single = 1.8F

			' Token: 0x0400137F RID: 4991
			Public Shared minRadiusSpeed As Single = 195F

			' Token: 0x04001380 RID: 4992
			Public Shared maxRadiusSpeed As Single = 365F

			' Token: 0x04001381 RID: 4993
			Public Shared radiusRampTime As Single = 1.8F

			' Token: 0x04001382 RID: 4994
			Public Shared damage As Single = 8F

			' Token: 0x04001383 RID: 4995
			Public Shared damageRate As Single = 0.3F

			' Token: 0x04001384 RID: 4996
			Public Shared maxDamage As Single = 37F

			' Token: 0x04001385 RID: 4997
			Public Shared freezeTime As Single = 0.1F
		End Class
	End Class

	' Token: 0x02000343 RID: 835
	Public NotInheritable Class LevelWeaponWideShot
		' Token: 0x170001CE RID: 462
		' (get) Token: 0x06000924 RID: 2340 RVA: 0x0007B841 File Offset: 0x00079C41
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.level_weapon_wide_shot)
			End Get
		End Property

		' Token: 0x170001CF RID: 463
		' (get) Token: 0x06000925 RID: 2341 RVA: 0x0007B84D File Offset: 0x00079C4D
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.level_weapon_wide_shot)
			End Get
		End Property

		' Token: 0x170001D0 RID: 464
		' (get) Token: 0x06000926 RID: 2342 RVA: 0x0007B859 File Offset: 0x00079C59
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.level_weapon_wide_shot)
			End Get
		End Property

		' Token: 0x04001386 RID: 4998
		Public Shared value As Integer = 4

		' Token: 0x04001387 RID: 4999
		Public Shared iconPath As String = "Icons/equip_icon_weapon_wide_shot"

		' Token: 0x04001388 RID: 5000
		Public Shared id As Weapon = Weapon.level_weapon_wide_shot

		' Token: 0x02000344 RID: 836
		Public NotInheritable Class Basic
			' Token: 0x04001389 RID: 5001
			Public Shared damage As Single = 2.67F

			' Token: 0x0400138A RID: 5002
			Public Shared speed As Single = 1800F

			' Token: 0x0400138B RID: 5003
			Public Shared distance As Single = 2000F

			' Token: 0x0400138C RID: 5004
			Public Shared rapidFireRate As Single = 0.22F

			' Token: 0x0400138D RID: 5005
			Public Shared angleRange As MinMax = New MinMax(50F, 8F)

			' Token: 0x0400138E RID: 5006
			Public Shared closingAngleSpeed As Single = 1.1F

			' Token: 0x0400138F RID: 5007
			Public Shared openingAngleSpeed As Single = 1.8F

			' Token: 0x04001390 RID: 5008
			Public Shared projectileSpeed As Single = 2F
		End Class

		' Token: 0x02000345 RID: 837
		Public NotInheritable Class Ex
			' Token: 0x04001391 RID: 5009
			Public Shared exDamage As Single = 21F

			' Token: 0x04001392 RID: 5010
			Public Shared exDuration As Single = 0.3F

			' Token: 0x04001393 RID: 5011
			Public Shared exHeight As Single = 86.5F
		End Class
	End Class

	' Token: 0x02000346 RID: 838
	Public NotInheritable Class PlaneSuperBomb
		' Token: 0x170001D1 RID: 465
		' (get) Token: 0x0600092A RID: 2346 RVA: 0x0007B90B File Offset: 0x00079D0B
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.plane_super_bomb)
			End Get
		End Property

		' Token: 0x170001D2 RID: 466
		' (get) Token: 0x0600092B RID: 2347 RVA: 0x0007B917 File Offset: 0x00079D17
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.plane_super_bomb)
			End Get
		End Property

		' Token: 0x170001D3 RID: 467
		' (get) Token: 0x0600092C RID: 2348 RVA: 0x0007B923 File Offset: 0x00079D23
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.plane_super_bomb)
			End Get
		End Property

		' Token: 0x04001394 RID: 5012
		Public Shared value As Integer = 10

		' Token: 0x04001395 RID: 5013
		Public Shared iconPath As String = "Icons/"

		' Token: 0x04001396 RID: 5014
		Public Shared id As Super = Super.plane_super_bomb

		' Token: 0x04001397 RID: 5015
		Public Shared damage As Single = 38F

		' Token: 0x04001398 RID: 5016
		Public Shared damageRate As Single = 0.25F

		' Token: 0x04001399 RID: 5017
		Public Shared countdownTime As Single = 3F
	End Class

	' Token: 0x02000347 RID: 839
	Public NotInheritable Class PlaneSuperChaliceSuperBomb
		' Token: 0x170001D4 RID: 468
		' (get) Token: 0x0600092E RID: 2350 RVA: 0x0007B96A File Offset: 0x00079D6A
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Super.plane_super_chalice_bomb)
			End Get
		End Property

		' Token: 0x170001D5 RID: 469
		' (get) Token: 0x0600092F RID: 2351 RVA: 0x0007B976 File Offset: 0x00079D76
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Super.plane_super_chalice_bomb)
			End Get
		End Property

		' Token: 0x170001D6 RID: 470
		' (get) Token: 0x06000930 RID: 2352 RVA: 0x0007B982 File Offset: 0x00079D82
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Super.plane_super_chalice_bomb)
			End Get
		End Property

		' Token: 0x0400139A RID: 5018
		Public Shared value As Integer = 10

		' Token: 0x0400139B RID: 5019
		Public Shared iconPath As String = "Icons/"

		' Token: 0x0400139C RID: 5020
		Public Shared id As Super = Super.plane_super_chalice_bomb

		' Token: 0x0400139D RID: 5021
		Public Shared damage As Single = 25.5F

		' Token: 0x0400139E RID: 5022
		Public Shared damageRate As Single = 0.25F

		' Token: 0x0400139F RID: 5023
		Public Shared turnRate As Single = 1F

		' Token: 0x040013A0 RID: 5024
		Public Shared maxAngle As Single = 60F

		' Token: 0x040013A1 RID: 5025
		Public Shared angleDamp As Single = 0.98F

		' Token: 0x040013A2 RID: 5026
		Public Shared accel As Single = 600F
	End Class

	' Token: 0x02000348 RID: 840
	Public NotInheritable Class PlaneWeaponBomb
		' Token: 0x170001D7 RID: 471
		' (get) Token: 0x06000932 RID: 2354 RVA: 0x0007B9F4 File Offset: 0x00079DF4
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.plane_weapon_bomb)
			End Get
		End Property

		' Token: 0x170001D8 RID: 472
		' (get) Token: 0x06000933 RID: 2355 RVA: 0x0007BA00 File Offset: 0x00079E00
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.plane_weapon_bomb)
			End Get
		End Property

		' Token: 0x170001D9 RID: 473
		' (get) Token: 0x06000934 RID: 2356 RVA: 0x0007BA0C File Offset: 0x00079E0C
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.plane_weapon_bomb)
			End Get
		End Property

		' Token: 0x040013A3 RID: 5027
		Public Shared value As Integer = 2

		' Token: 0x040013A4 RID: 5028
		Public Shared iconPath As String = "Icons/"

		' Token: 0x040013A5 RID: 5029
		Public Shared id As Weapon = Weapon.plane_weapon_bomb

		' Token: 0x02000349 RID: 841
		Public NotInheritable Class Basic
			' Token: 0x040013A6 RID: 5030
			Public Shared damage As Single = 11.5F

			' Token: 0x040013A7 RID: 5031
			Public Shared speed As Single = 1200F

			' Token: 0x040013A8 RID: 5032
			Public Shared Up As Boolean

			' Token: 0x040013A9 RID: 5033
			Public Shared sizeExplosion As Single = 1F

			' Token: 0x040013AA RID: 5034
			Public Shared size As Single = 1F

			' Token: 0x040013AB RID: 5035
			Public Shared angle As Single = 45F

			' Token: 0x040013AC RID: 5036
			Public Shared gravity As Single = 4500F

			' Token: 0x040013AD RID: 5037
			Public Shared rapidFire As Boolean = True

			' Token: 0x040013AE RID: 5038
			Public Shared rapidFireRate As Single = 0.6F
		End Class

		' Token: 0x0200034A RID: 842
		Public NotInheritable Class Ex
			' Token: 0x040013AF RID: 5039
			Public Shared damage As Single = 6F

			' Token: 0x040013B0 RID: 5040
			Public Shared speed As Single = 700F

			' Token: 0x040013B1 RID: 5041
			Public Shared angles As Single() = New Single() { 180F, 170F }

			' Token: 0x040013B2 RID: 5042
			Public Shared counts As Integer() = New Integer() { 6, 3 }

			' Token: 0x040013B3 RID: 5043
			Public Shared rotationSpeed As MinMax = New MinMax(0F, 250F)

			' Token: 0x040013B4 RID: 5044
			Public Shared timeBeforeEaseRotationSpeed As Single = 0F

			' Token: 0x040013B5 RID: 5045
			Public Shared rotationSpeedEaseTime As Single = 1F

			' Token: 0x040013B6 RID: 5046
			Public Shared maxHomingTime As Single = 2.5F
		End Class
	End Class

	' Token: 0x0200034B RID: 843
	Public NotInheritable Class PlaneWeaponChaliceBomb
		' Token: 0x170001DA RID: 474
		' (get) Token: 0x06000938 RID: 2360 RVA: 0x0007BB11 File Offset: 0x00079F11
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.plane_chalice_weapon_bomb)
			End Get
		End Property

		' Token: 0x170001DB RID: 475
		' (get) Token: 0x06000939 RID: 2361 RVA: 0x0007BB1D File Offset: 0x00079F1D
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.plane_chalice_weapon_bomb)
			End Get
		End Property

		' Token: 0x170001DC RID: 476
		' (get) Token: 0x0600093A RID: 2362 RVA: 0x0007BB29 File Offset: 0x00079F29
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.plane_chalice_weapon_bomb)
			End Get
		End Property

		' Token: 0x040013B7 RID: 5047
		Public Shared value As Integer = 10

		' Token: 0x040013B8 RID: 5048
		Public Shared iconPath As String = "Icons/equip_icon_chalice_shmup_bomb"

		' Token: 0x040013B9 RID: 5049
		Public Shared id As Weapon = Weapon.plane_chalice_weapon_bomb

		' Token: 0x0200034C RID: 844
		Public NotInheritable Class Basic
			' Token: 0x040013BA RID: 5050
			Public Shared damage As Single = 6.6F

			' Token: 0x040013BB RID: 5051
			Public Shared size As Single = 1F

			' Token: 0x040013BC RID: 5052
			Public Shared sizeExplosion As Single = 1F

			' Token: 0x040013BD RID: 5053
			Public Shared angleRange As Single = 35F

			' Token: 0x040013BE RID: 5054
			Public Shared gravity As Single = 1700F

			' Token: 0x040013BF RID: 5055
			Public Shared speed As Single = 700F

			' Token: 0x040013C0 RID: 5056
			Public Shared rapidFire As Boolean = True

			' Token: 0x040013C1 RID: 5057
			Public Shared rapidFireRate As Single = 0.2F

			' Token: 0x040013C2 RID: 5058
			Public Shared damageExplosion As Single = 2.5F
		End Class

		' Token: 0x0200034D RID: 845
		Public NotInheritable Class Ex
			' Token: 0x040013C3 RID: 5059
			Public Shared damage As Single = 15.5F

			' Token: 0x040013C4 RID: 5060
			Public Shared damageRate As Single = 0.17F

			' Token: 0x040013C5 RID: 5061
			Public Shared damageRateIncrease As Single = 0.07F

			' Token: 0x040013C6 RID: 5062
			Public Shared startSpeed As Single = 600F

			' Token: 0x040013C7 RID: 5063
			Public Shared gravity As Single = 1900F

			' Token: 0x040013C8 RID: 5064
			Public Shared freezeTime As Single = 0.125F
		End Class
	End Class

	' Token: 0x0200034E RID: 846
	Public NotInheritable Class PlaneWeaponChaliceWay
		' Token: 0x170001DD RID: 477
		' (get) Token: 0x0600093E RID: 2366 RVA: 0x0007BBF5 File Offset: 0x00079FF5
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.plane_chalice_weapon_3way)
			End Get
		End Property

		' Token: 0x170001DE RID: 478
		' (get) Token: 0x0600093F RID: 2367 RVA: 0x0007BC01 File Offset: 0x0007A001
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.plane_chalice_weapon_3way)
			End Get
		End Property

		' Token: 0x170001DF RID: 479
		' (get) Token: 0x06000940 RID: 2368 RVA: 0x0007BC0D File Offset: 0x0007A00D
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.plane_chalice_weapon_3way)
			End Get
		End Property

		' Token: 0x040013C9 RID: 5065
		Public Shared value As Integer = 10

		' Token: 0x040013CA RID: 5066
		Public Shared iconPath As String = "Icons/equip_icon_chalice_shmup_3way"

		' Token: 0x040013CB RID: 5067
		Public Shared id As Weapon = Weapon.plane_chalice_weapon_3way

		' Token: 0x0200034F RID: 847
		Public NotInheritable Class Basic
			' Token: 0x040013CC RID: 5068
			Public Shared damage As Single = 3.65F

			' Token: 0x040013CD RID: 5069
			Public Shared speed As Single = 1650F

			' Token: 0x040013CE RID: 5070
			Public Shared distance As Single

			' Token: 0x040013CF RID: 5071
			Public Shared rapidFireRate As Single = 0.23F

			' Token: 0x040013D0 RID: 5072
			Public Shared angle As Single = 9F
		End Class

		' Token: 0x02000350 RID: 848
		Public NotInheritable Class Ex
			' Token: 0x040013D1 RID: 5073
			Public Shared damageBeforeLaunch As Single = 2.4F

			' Token: 0x040013D2 RID: 5074
			Public Shared damageRateBeforeLaunch As Single = 0.25F

			' Token: 0x040013D3 RID: 5075
			Public Shared arcSpeed As Single = 5F

			' Token: 0x040013D4 RID: 5076
			Public Shared arcX As Single = 250F

			' Token: 0x040013D5 RID: 5077
			Public Shared arcY As Single = 40F

			' Token: 0x040013D6 RID: 5078
			Public Shared pauseTime As Single

			' Token: 0x040013D7 RID: 5079
			Public Shared damageAfterLaunch As Single = 17F

			' Token: 0x040013D8 RID: 5080
			Public Shared speedAfterLaunch As Single = -1250F

			' Token: 0x040013D9 RID: 5081
			Public Shared accelAfterLaunch As Single = 8000F

			' Token: 0x040013DA RID: 5082
			Public Shared freezeTime As Single = 0.125F

			' Token: 0x040013DB RID: 5083
			Public Shared minXDistance As Single = 75F

			' Token: 0x040013DC RID: 5084
			Public Shared xDistanceNoTarget As Integer = 500
		End Class
	End Class

	' Token: 0x02000351 RID: 849
	Public NotInheritable Class PlaneWeaponLaser
		' Token: 0x170001E0 RID: 480
		' (get) Token: 0x06000944 RID: 2372 RVA: 0x0007BCDB File Offset: 0x0007A0DB
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.plane_weapon_laser)
			End Get
		End Property

		' Token: 0x170001E1 RID: 481
		' (get) Token: 0x06000945 RID: 2373 RVA: 0x0007BCE7 File Offset: 0x0007A0E7
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.plane_weapon_laser)
			End Get
		End Property

		' Token: 0x170001E2 RID: 482
		' (get) Token: 0x06000946 RID: 2374 RVA: 0x0007BCF3 File Offset: 0x0007A0F3
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.plane_weapon_laser)
			End Get
		End Property

		' Token: 0x040013DD RID: 5085
		Public Shared value As Integer = 2

		' Token: 0x040013DE RID: 5086
		Public Shared iconPath As String = "Icons/"

		' Token: 0x040013DF RID: 5087
		Public Shared id As Weapon = Weapon.plane_weapon_laser

		' Token: 0x02000352 RID: 850
		Public NotInheritable Class Basic
			' Token: 0x040013E0 RID: 5088
			Public Shared damage As Single = 8F

			' Token: 0x040013E1 RID: 5089
			Public Shared speed As Single = 4000F

			' Token: 0x040013E2 RID: 5090
			Public Shared rapidFire As Boolean = True

			' Token: 0x040013E3 RID: 5091
			Public Shared rapidFireRate As Single = 0.1F
		End Class

		' Token: 0x02000353 RID: 851
		Public NotInheritable Class Ex
			' Token: 0x040013E4 RID: 5092
			Public Shared damage As Single = 3F

			' Token: 0x040013E5 RID: 5093
			Public Shared speed As Single = 2000F

			' Token: 0x040013E6 RID: 5094
			Public Shared angles As Single() = New Single() { 180F, 170F }

			' Token: 0x040013E7 RID: 5095
			Public Shared counts As Integer() = New Integer() { 12, 6 }
		End Class
	End Class

	' Token: 0x02000354 RID: 852
	Public NotInheritable Class PlaneWeaponPeashot
		' Token: 0x170001E3 RID: 483
		' (get) Token: 0x0600094A RID: 2378 RVA: 0x0007BD94 File Offset: 0x0007A194
		Public Shared ReadOnly Property displayName As String
			Get
				Return WeaponProperties.GetDisplayName(Weapon.plane_weapon_peashot)
			End Get
		End Property

		' Token: 0x170001E4 RID: 484
		' (get) Token: 0x0600094B RID: 2379 RVA: 0x0007BDA0 File Offset: 0x0007A1A0
		Public Shared ReadOnly Property subtext As String
			Get
				Return WeaponProperties.GetSubtext(Weapon.plane_weapon_peashot)
			End Get
		End Property

		' Token: 0x170001E5 RID: 485
		' (get) Token: 0x0600094C RID: 2380 RVA: 0x0007BDAC File Offset: 0x0007A1AC
		Public Shared ReadOnly Property description As String
			Get
				Return WeaponProperties.GetDescription(Weapon.plane_weapon_peashot)
			End Get
		End Property

		' Token: 0x040013E8 RID: 5096
		Public Shared value As Integer = 2

		' Token: 0x040013E9 RID: 5097
		Public Shared iconPath As String = "Icons/equip_icon_weapon_peashot"

		' Token: 0x040013EA RID: 5098
		Public Shared id As Weapon = Weapon.plane_weapon_peashot

		' Token: 0x02000355 RID: 853
		Public NotInheritable Class Basic
			' Token: 0x040013EB RID: 5099
			Public Shared damage As Single = 4F

			' Token: 0x040013EC RID: 5100
			Public Shared speed As Single = 1800F

			' Token: 0x040013ED RID: 5101
			Public Shared rapidFire As Boolean = True

			' Token: 0x040013EE RID: 5102
			Public Shared rapidFireRate As Single = 0.07F
		End Class

		' Token: 0x02000356 RID: 854
		Public NotInheritable Class Ex
			' Token: 0x040013EF RID: 5103
			Public Shared damage As Single = 15F

			' Token: 0x040013F0 RID: 5104
			Public Shared damageDistance As Single = 100F

			' Token: 0x040013F1 RID: 5105
			Public Shared acceleration As Single = 2500F

			' Token: 0x040013F2 RID: 5106
			Public Shared maxSpeed As Single = 1500F

			' Token: 0x040013F3 RID: 5107
			Public Shared freezeTime As Single = 0.125F
		End Class
	End Class
End Module
