# SuperBreakout Game Project

## プロジェクト概要
SuperBreakoutは、クラシックなブロック崩しゲームに現代的なゲームプレイ要素を加えたUnityプロジェクトです。

## ゲーム仕様

### コアゲームプレイ
- プレイヤーは画面下部のパドルを左右に操作
- ボールを反射させてブロックを破壊
- すべてのブロックを破壊するとステージクリア
- ボールを落とすとライフが減少
- ライフが0になるとゲームオーバー

### ゲーム要素
1. **パドル操作**
   - キーボード（A/D または 左右矢印キー）で移動
   - マウス追従モードも実装

2. **ボール物理**
   - リアルな物理挙動
   - パドルの当たる位置で反射角度が変化
   - 速度の最小値保証（止まらないように）

3. **ブロックシステム**
   - 複数の耐久度（1-3ヒット）
   - 色で耐久度を表現
   - 破壊時のパーティクルエフェクト

4. **スコアシステム**
   - ブロック破壊でポイント獲得
   - 耐久度が高いブロックほど高得点
   - コンボシステム（連続破壊でボーナス）

5. **パワーアップ**
   - マルチボール
   - パドル拡大/縮小
   - ボール速度アップ/ダウン
   - 貫通ボール

## ディレクトリ構造
```
SuperBreakout/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs
│   │   │   ├── ScoreManager.cs
│   │   │   └── AudioManager.cs
│   │   ├── Player/
│   │   │   └── PaddleController.cs
│   │   ├── Ball/
│   │   │   └── BallController.cs
│   │   ├── Blocks/
│   │   │   ├── Block.cs
│   │   │   └── BlockSpawner.cs
│   │   ├── PowerUps/
│   │   │   ├── PowerUp.cs
│   │   │   └── PowerUpTypes/
│   │   └── UI/
│   │       ├── UIManager.cs
│   │       └── GameOverUI.cs
│   ├── Prefabs/
│   │   ├── Player/
│   │   ├── Ball/
│   │   ├── Blocks/
│   │   ├── PowerUps/
│   │   └── UI/
│   ├── Materials/
│   │   ├── Player/
│   │   ├── Ball/
│   │   └── Blocks/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   └── GameScene.unity
│   └── Audio/
│       ├── SFX/
│       └── Music/
├── ProjectSettings/
├── Packages/
└── .gitignore
```

## 開発手順
1. GitHubリポジトリの作成と初期設定
2. Unityプロジェクトの基本セットアップ
3. ゲームシーンの作成
4. プレイヤー（パドル）の実装
5. ボールの物理挙動実装
6. ブロックシステムの実装
7. ゲームマネージャーとスコアシステム
8. UI実装
9. パワーアップシステム
10. オーディオとエフェクトの追加

## 技術仕様
- Unity バージョン: 2022.3 LTS以降
- レンダリングパイプライン: URP (Universal Render Pipeline)
- 入力システム: 新しいInput System
- 物理: Unity標準の2D Physics

## ビルド設定
- プラットフォーム: Windows/Mac/Linux スタンドアロン
- 解像度: 1920x1080 (16:9)
- フレームレート: 60 FPS ターゲット

## Gitコマンド
```bash
# リポジトリの初期化
git init
git remote add origin https://github.com/IwakenLab/SuperBreakout.git

# コミット時
git add .
git commit -m "コミットメッセージ"
git push origin main

# ブランチ作成
git checkout -b feature/機能名
```

## テストコマンド
```bash
# Unity Test Runner実行
# Edit Modeテスト
mcp__unity-natural-mcp__RunEditModeTests

# Play Modeテスト
mcp__unity-natural-mcp__RunPlayModeTests
```