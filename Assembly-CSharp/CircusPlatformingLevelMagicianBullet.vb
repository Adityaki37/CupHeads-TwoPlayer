Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020008A7 RID: 2215
Public Class CircusPlatformingLevelMagicianBullet
	Inherits BasicProjectile

	' Token: 0x14000062 RID: 98
	' (add) Token: 0x0600339E RID: 13214 RVA: 0x001DFEF8 File Offset: 0x001DE2F8
	' (remove) Token: 0x0600339F RID: 13215 RVA: 0x001DFF30 File Offset: 0x001DE330
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnProjectileDeath As Action

	' Token: 0x17000446 RID: 1094
	' (get) Token: 0x060033A0 RID: 13216 RVA: 0x001DFF66 File Offset: 0x001DE366
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060033A1 RID: 13217 RVA: 0x001DFF70 File Offset: 0x001DE370
	Protected Overrides Sub Start()
		MyBase.Start()
		AudioManager.PlayLoop("circus_magician_magic_loop")
		Me.emitAudioFromObject.Add("circus_magician_magic_loop")
		Me.puffs.flipX = Rand.Bool()
		Me.puffs.flipY = Rand.Bool()
		Me.DestroyDistance = 0F
	End Sub

	' Token: 0x060033A2 RID: 13218 RVA: 0x001DFFC8 File Offset: 0x001DE3C8
	Protected Overrides Sub OnDestroy()
		AudioManager.[Stop]("circus_magician_magic_loop")
		If Me.OnProjectileDeath IsNot Nothing Then
			Me.OnProjectileDeath()
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04003BEF RID: 15343
	<SerializeField()>
	Private puffs As SpriteRenderer
End Class
