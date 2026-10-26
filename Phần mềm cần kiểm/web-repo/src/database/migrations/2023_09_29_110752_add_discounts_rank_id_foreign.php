<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::table('discounts',function(Blueprint $table) {
            $table
            ->foreign('rank_id')
            ->references('id')
            ->on('ranks');
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::table('discounts',function(Blueprint $table) {
            $table->dropForeign('discounts_rank_id_foreign');
        });
    }
};
