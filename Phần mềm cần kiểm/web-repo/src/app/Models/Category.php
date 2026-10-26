<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use DB;
use App\Models\Product;

class Category extends Model
{
    use HasFactory;
    protected $fillable = [
        'name',
        'description',
    ];
    protected $table = 'categories';
    public function getList(){
        $categories = categories::all();
        return $categories;
    }
    public function add($data){
        return categories::insert($data);
    }
    public function getDetail($id){
        return DB::table($this->table)
        ->where('id', $id)
        ->first();
    }
    public function updateItem($id, $data){
        return categories::where('id',$id)->update($data);
    }
    public function deleteItem($id){
        return categories::where('id',$id)->delete();
    }
    public function product(){
        return $this->hasMany(Product::class);
    }
}
