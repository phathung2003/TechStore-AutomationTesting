<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use App\Models\Product;
use DB;

class Favorite extends Model
{
    use HasFactory;
    public function products(){
        return $this->hasMany(Product::class);
    }
    public function deleteItem($id){
        return Favorite::where('product_id',$id)->delete();
    }
}
