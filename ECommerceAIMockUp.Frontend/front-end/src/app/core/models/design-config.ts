export interface DesignConfig {
  id: string;
  imageUrl: string;
  position?: {
    x: number;
    y: number;
    constraint?: 'front' | 'back' | 'sleeve';
  };
  transform?: {
    scaleX?: number;
    scaleY?: number;
    rotation?: number;
    flipX?: boolean;
    flipY?: boolean;
    opacity?: number;
  };
  zIndex?: number;
}
