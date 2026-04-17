Imports System
Imports UnityEngine

' Token: 0x02000A5B RID: 2651
Public Class PlayerSuperGhostHeart
	Inherits AbstractLevelEntity

	' Token: 0x06003F30 RID: 16176 RVA: 0x00229850 File Offset: 0x00227C50
	Private Sub FixedUpdate()
		MyBase.transform.AddPosition(0F, WeaponProperties.LevelSuperGhost.heartSpeed * CupheadTime.FixedDelta * Me.gravityMultiplier, 0F)
		If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(50F, 50F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003F31 RID: 16177 RVA: 0x002298C0 File Offset: 0x00227CC0
	Public Function Create(pos As Vector2, gravityMultiplier As Single) As PlayerSuperGhostHeart
		Dim playerSuperGhostHeart As PlayerSuperGhostHeart = Me.InstantiatePrefab(Of PlayerSuperGhostHeart)()
		playerSuperGhostHeart.transform.position = pos
		playerSuperGhostHeart.transform.localScale = New Vector3(playerSuperGhostHeart.transform.localScale.x, gravityMultiplier, playerSuperGhostHeart.transform.localScale.z)
		playerSuperGhostHeart.gravityMultiplier = gravityMultiplier
		Return playerSuperGhostHeart
	End Function

	' Token: 0x06003F32 RID: 16178 RVA: 0x00229924 File Offset: 0x00227D24
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		player.stats.AddEx()
		Me.spark.Create(MyBase.transform.position)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04004644 RID: 17988
	Private gravityMultiplier As Single

	' Token: 0x04004645 RID: 17989
	<SerializeField()>
	Private spark As Effect
End Class
