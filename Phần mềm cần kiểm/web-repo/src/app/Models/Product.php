<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use App\Models\Comment;
use App\Models\Category;
use App\Models\Brand;
use App\Models\Favorite;
class Product extends Model
{
    use HasFactory;
    protected $fillable = [
        'user_id',
        'product_id',
        'content'
    ];

    public function comments(){
        return $this->hasMany(Comment::class);
    }
    public function category(){
        return $this->belongsTo(Category::class);
    }
    public function brand(){
        return $this->belongsTo(Brand::class);
    }
    public function favorites(){
        return $this->belongsTo(Favorite::class);
    }
}
